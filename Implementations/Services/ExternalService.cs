using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Services;
public class ExternalService : IExternalService
{
    IExternalRepo _externalRepo;
    IHardwareService _hardwareService;
    IRealtimeNotificationService _realtime;
    IMqttService _mqtt;
    public ExternalService(IExternalRepo externalRepo, IHardwareService hardwareService, IRealtimeNotificationService realtime, IMqttService mqtt)
    {
        _externalRepo = externalRepo;
        _hardwareService = hardwareService;
        _realtime = realtime;
        _mqtt = mqtt;
    }
    public async Task<BaseResponse> CreateExternal(CreateExternalDto createExternalDto, int personId)
    {
        if (createExternalDto != null)
        {
            var external = new External()
            {
                ExternalId = $"Externals{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                ExternalName = createExternalDto.ExternalName,
                SectionId = createExternalDto.SectionId ?? null,
                RoomId = createExternalDto.RoomId ?? null,
                IsDeleted = false,
                CreatedBy = personId,
                LastModifiedBy = personId,
                LastModifiedOn = DateTime.Now,
                CreatedOn = DateTime.Now,
                ActionIds = createExternalDto.ActionIds ?? new List<int>(),
                IsTriggered = false,
                IsActive = true,
                PowerActive = true,
            };
            await _externalRepo.Create(external);
            var externalData = GetDetails(await _externalRepo.Get(x => x.ExternalId == external.ExternalId));
            if(externalData.IsHidden){ await _realtime.NotifyAll("ExternalsCreated", new List<Role> { Role.Owner, Role.Wife }, externalData);}
            else await _realtime.NotifyAll("ExternalsCreated", externalData);
            return new BaseResponse()
            {
                Status = true,
                Message = "External Successfully Added!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Add External!"
        };
    }

    public async Task<BaseResponse> UpdateExternal(UpdateExternalDto updateExternalDto, int personId)
    {
        if (updateExternalDto != null)
        {
            var Externals = await _externalRepo.Get(x => x.Id == updateExternalDto.Id);
            if (Externals != null)
            {
                Externals.ExternalName = updateExternalDto.ExternalName ?? Externals.ExternalName;
                Externals.LastModifiedBy = personId;
                Externals.LastModifiedOn = DateTime.Now;
                Externals.IsActive = updateExternalDto.IsActive;
                Externals.SectionId = updateExternalDto.SectionId ?? null;
                Externals.RoomId = updateExternalDto.RoomId ?? null;
                Externals.IsTriggered = updateExternalDto.IsTriggered;
                Externals.ActionIds = updateExternalDto.ActionIds ?? Externals.ActionIds;
                var externalData = await _externalRepo.Update(Externals);
                var hardware = await _hardwareService.GetHardwareName(Externals.Id, FacilityType.External);
                if(externalData.IsHidden){ await _realtime.NotifyAll("ExternalsUpdated", new List<Role> { Role.Owner, Role.Wife }, GetDetails(externalData));}
                else await _realtime.NotifyAll("ExternalsUpdated", GetDetails(externalData));
                await _mqtt.PublishDeviceUpdate(Externals.ExternalId, new GenericDeviceUpdateState
                {
                    FacilityType = FacilityType.External,
                    Id = externalData.Id,
                    BrightnessLevel = 0,
                    IsActive = externalData.IsActive,
                    PowerActive = externalData.PowerActive,
                    IsLocked = false,
                    IsOpen = false
                });
                return new BaseResponse()
                {
                    Status = true,
                    Message = "Externals Updated Successfully!"
                };
            }
            return new BaseResponse()
            {
                Status = false,
                Message = "Externals Not Found!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update Externals!"
        };
    }
    public async Task<ExternalResponseModel> GetById(int ExternalsId)
    {
        var external = await _externalRepo.Get(x => x.Id == ExternalsId && x.IsDeleted == false);
        if (external != null)
        {
            return new ExternalResponseModel()
            {
                Data = GetDetails(external),
                Status = true,
                Message = "External Retrieved Successfully!"
            };
        }
        return new ExternalResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Externals!"
        };
    }

    public async Task<ExternalsResponseModel> GetAllExternals()
    {
        var externals = await _externalRepo.GetByExpression(x => x.IsDeleted == false);
        if (externals != null)
        {
            return new ExternalsResponseModel()
            {
                Data = externals.Select(x => GetDetails(x)).ToList(),
                Message = "Externals Retrieved Successfully!",
                Status = true
            };
        }
        return new ExternalsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Externals!",
            Status = false
        };
    }

    public async Task<ExternalsResponseModel> GetAllExternalsByRoomId(int roomId)
    {
        var externals = await _externalRepo.GetByExpression(x => x.RoomId == roomId && x.IsDeleted == false);

        if (externals != null)
        {
            return new ExternalsResponseModel()
            {
                Data = externals.Select(x => GetDetails(x)).ToList(),
                Message = "Externals Retrieved Successfully!",
                Status = true
            };
        }
        return new ExternalsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Externals!",
            Status = false
        };
    }
    public async Task<ExternalsResponseModel> GetAllExternalsBySectionId(int sectionId)
    {
        var externals = await _externalRepo.GetByExpression(x => x.SectionId == sectionId && x.IsDeleted == false);
        if (externals != null)
        {
            return new ExternalsResponseModel()
            {
                Data = externals.Select(x => GetDetails(x)).ToList(),
                Message = "Externals Retrieved Successfully!",
                Status = true
            };
        }
        return new ExternalsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Externals!",
            Status = false
        };
    }
    public async Task<BaseResponse> Delete(int ExternalsId, int personId)
    {
        var external = await _externalRepo.Get(x => x.Id == ExternalsId);

        if (external != null)
        {
            external.IsActive = false;
            external.PowerActive = false;
            external.LastModifiedBy = personId;
            external.LastModifiedOn = DateTime.Now;
            external.DeletedOn = DateTime.Now;
            external.DeletedBy = personId;
            external.IsDeleted = true;
            await _externalRepo.Update(external);
            await _hardwareService.RemoveFacilityItem(external.Id, FacilityType.External);
            await _realtime.NotifyAll("ExternalsDeleted", new
            {
                external.Id,
            });
            await _mqtt.PublishDeviceUpdate(external.ExternalId, new GenericDeviceUpdateState
            {
                FacilityType = FacilityType.External,
                Id = external.Id,
                BrightnessLevel = 0,
                IsActive = false,
                PowerActive = false,
                IsLocked = false,
                IsOpen = false
            });
            return new BaseResponse()
            {
                Status = true,
                Message = "External Deleted Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete External!"
        };
    }
    public GetExternalDto GetDetails(External External)
    {
        return new GetExternalDto()
        {
            Id = External.Id,
            ExternalName = External.ExternalName,
            IsTriggered = External.IsTriggered,
            IsActive = External.IsActive,
            PowerActive = External.PowerActive,
            RoomId = External.RoomId,
            SectionId = External.SectionId,
            ExternalId = External.ExternalId,
            LastModifiedBy = External.LastModifiedBy,
            DeletedBy = External.DeletedBy,
            CreatedBy = External.CreatedBy,
            LastModifiedOn = External.LastModifiedOn,
            CreatedOn = External.CreatedOn,
            IsDeleted = External.IsDeleted,
            IsHidden = External.IsHidden,
            ActionIds = External.ActionIds ?? new List<int>(),
        };
    }
}