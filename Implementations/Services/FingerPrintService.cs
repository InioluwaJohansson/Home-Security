using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Entities;
using SourceAFIS;
using Home_Security.RealTimeServices;
using Home_Security.Entities.Chats;
using Home_Security.Entities.Identity;

namespace Home_Security.Implementations.Services;
public class FingerPrintService : IFingerPrintService
{
    IFingerPrintRepo _fingerPrintRepo;
    IPersonRepo _personRepo;
    IUserRepo _userRepo;
    IHardwareRepo _hardwareRepo;
    IRealtimeNotificationService _realtimeNotificationService;
    IMqttService _mqttService;
    public FingerPrintService(IFingerPrintRepo fingerPrintRepo, IPersonRepo personRepo, IUserRepo userRepo, IHardwareRepo hardwareRepo, IRealtimeNotificationService realtimeNotificationService, IMqttService mqttService)
    {
        _fingerPrintRepo = fingerPrintRepo;
        _personRepo = personRepo;
        _hardwareRepo = hardwareRepo;
        _realtimeNotificationService = realtimeNotificationService;
        _userRepo = userRepo;
        _mqttService = mqttService;
    }
    public async Task<FingerPrintResponseModel> VerifyFingerPrint(byte[] data)
    {
        var allFingerprints = await _fingerPrintRepo.GetByExpression(x => x.IsDeleted == false);
        if (allFingerprints == null || !allFingerprints.Any())
        {
            return new FingerPrintResponseModel
            {
                Status = false,
                Message = "No fingerprints found"
            };
        }
        var matcher = new FingerprintMatcher(new FingerprintTemplate(new FingerprintImage(data)));
        double bestScore = 0;
        FingerPrint bestMatch = null;
        foreach (var fp in allFingerprints)
        {
            double score = matcher.Match(new FingerprintTemplate(fp.FingerPrintEncoding));
            if (score > bestScore)
            {
                bestScore = score;
                bestMatch = fp;
            }
        }
        if (bestScore >= 40 && bestMatch != null)
        {
            return new FingerPrintResponseModel
            {
                Status = true,
                Message = $"Fingerprint matched (score: {bestScore})",
                //Data = new GetFingerPrintDto
                // {
                //     Id = bestMatch.Id,
                //     PersonId = bestMatch.PersonId,
                //     FingerPrintId = bestMatch.FingerPrintId
                // }
            };
        }
        return new FingerPrintResponseModel
        {
            Status = false,
            Message = $"No match found (best score: {bestScore})"
        };
    }
    public async Task<FingerPrintResponseModel> CreateFingerPrint(CreateFingerPrintDto createFingerPrintDto, int personId)
    {
        if(createFingerPrintDto != null){
            foreach(var fingerprint in createFingerPrintDto.FingerPrintEncoding){
                var template = new FingerprintTemplate(new FingerprintImage(fingerprint));
                var entity = new FingerPrint
                {
                    PersonId = createFingerPrintDto.PersonId,
                    FingerPrintEncoding = template.ToByteArray(),
                    FingerPrintId = Guid.NewGuid().ToString(),
                    IsDeleted = false,
                    LastModifiedBy = personId,
                    LastModifiedOn = DateTime.UtcNow,
                    CreatedBy = personId,
                    CreatedOn = DateTime.UtcNow
                };
                await _fingerPrintRepo.Create(entity);
            }
            return new FingerPrintResponseModel
            {
                Status = true,
                Message = "Fingerprints enrolled successfully",
                Data = new GetFingerPrintDto
                {
                    PersonId = createFingerPrintDto.PersonId,
                }
            };
        }
        return new FingerPrintResponseModel
        {
            Status = false,
            Message = "Failed to enroll fingerprints"
        };
    }
    public async Task<BaseResponse> RequestFingerPrint(int personId, int hardwareId)
    {
        var person = await _personRepo.Get(x => x.Id == personId);
        var hardware = await _hardwareRepo.Get(x => x.Id == hardwareId);
        if(person != null && hardware != null)
        {
            await _mqttService.PublishFingerPrint(hardware.HardwareId, new FingerPrintPayload
            {
                PersonId = person.Id,
                Message = $"Requesting fingerprint enrollment"
            });
            return new BaseResponse
            {
                Status = true,
                Message = "Fingerprint enrollment request sent! Awaiting data..."
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Failed to send fingerprint enrollment request!"
        };
    }
    public async Task ReceiveFingerPrint(FingerPrintDataDto fingerPrintDataDto)
    {
        await _realtimeNotificationService.NotifyAll("fingerprint_received", new List<Role> { Role.Owner }, new
        {
            PersonId = fingerPrintDataDto.PersonId,
            Message = "Fingerprint data received",
            FingerPrintEncoding = fingerPrintDataDto.FingerPrintEncoding
        });
    }
    public async Task<BaseResponse> SendNFIDCode(int personId, int hardwareId)
    {
        var user = await _userRepo.Get(x => x.Id == personId);
        var hardware = await _hardwareRepo.Get(x => x.Id == hardwareId);
        if(user != null && hardware != null)
        {
            await _mqttService.PublishNFIDCode(hardware.HardwareId, user.NFIDCode);
            return new BaseResponse
            {
                Status = true,
                Message = "NFID code sent to hardware! Kindly place the NFID Card on the scanner within 30 seconds to complete verification."
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Failed to send NFID code!"
        };
    }
}