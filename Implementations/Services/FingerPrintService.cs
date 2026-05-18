using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Entities;
using SourceAFIS;
namespace Home_Security.Implementations.Services;

public class FingerPrintService : IFingerPrintService
{
    IFingerPrintRepo _fingerPrintRepo;
    public FingerPrintService(IFingerPrintRepo fingerPrintRepo)
    {
        _fingerPrintRepo = fingerPrintRepo;
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
                Data = new GetFingerPrintDto
                {
                    Id = bestMatch.Id,
                    PersonId = bestMatch.PersonId,
                    FingerPrintId = bestMatch.FingerPrintId
                }
            };
        }
        return new FingerPrintResponseModel
        {
            Status = false,
            Message = $"No match found (best score: {bestScore})"
        };
    }
    public async Task<FingerPrintResponseModel> CreateFingerPrint(CreateFingerPrintDto createFingerPrintDto)
    {
        var template = new FingerprintTemplate(new FingerprintImage(createFingerPrintDto.FingerPrintEncoding));
        var entity = new FingerPrint
        {
            PersonId = createFingerPrintDto.PersonId,
            FingerPrintEncoding = template.ToByteArray(),
            FingerPrintId = Guid.NewGuid().ToString(),
            IsDeleted = false
        };
        await _fingerPrintRepo.Create(entity);
        return new FingerPrintResponseModel
        {
            Status = true,
            Message = "Fingerprint enrolled successfully",
            Data = new GetFingerPrintDto
            {
                Id = entity.Id,
                PersonId = entity.PersonId,
                FingerPrintId = entity.FingerPrintId
            }
        };
    }
}