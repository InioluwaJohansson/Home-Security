using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Services;

public class ExternalService : IExternalService
{
    IExternalRepo _externalRepo;
    RealtimeNotificationService _realtime;
    IMqttService _mqtt;

    public ExternalService(IExternalRepo externalRepo, RealtimeNotificationService realtime, IMqttService mqtt)
    {
        _externalRepo = externalRepo;
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
                SectionId = createExternalDto.SectionId,
                RoomId = createExternalDto.RoomId,
                IsDeleted = false,
                CreatedBy = personId,
                LastModifiedBy = personId,
                LastModifiedOn = DateTime.Now,
                CreatedOn = DateTime.Now,
                ActionIds = createExternalDto.ActionIds,
                IsTriggered = false,
                IsActive = true,
                PowerActive = true,
            };

            await _externalRepo.Create(external);

            await _realtime.NotifyAll("ExternalsCreated", GetDetails(external));

            await _mqtt.PublishDeviceUpdate(external.ExternalId, new GenericDeviceUpdateState
            {
                FacilityType = FacilityType.External,
                Id = external.Id,
                BrightnessLevel = 0,
                IsActive = true,
                PowerActive = false,
                IsLocked = false,
                IsOpen = false
            });
            return new BaseResponse()
            {
                Status = true,
                Message = "Externals Successfully Added!"
            };
        }

        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Add Externals!"
        };
    }

    public async Task<BaseResponse> UpdateExternal(UpdateExternalDto updateExternalDto, int personId)
    {
        if (updateExternalDto != null)
        {
            var Externals = await _externalRepo.Get(x => x.Id == updateExternalDto.Id);

            Externals.ExternalName = updateExternalDto.ExternalName ?? Externals.ExternalName;
            Externals.LastModifiedBy = personId;
            Externals.LastModifiedOn = DateTime.Now;
            Externals.PowerActive = updateExternalDto.PowerActive;
            Externals.IsTriggered = updateExternalDto.IsTriggered;
            Externals.ActionIds = updateExternalDto.ActionIds ?? Externals.ActionIds;

            await _externalRepo.Update(Externals);

            await _realtime.NotifyAll("ExternalsUpdated", GetDetails(Externals));

            await _mqtt.PublishDeviceUpdate(Externals.ExternalId, new GenericDeviceUpdateState
            {
                FacilityType = FacilityType.External,
                Id = Externals.Id,
                BrightnessLevel = 0,
                IsActive = Externals.IsActive,
                PowerActive = Externals.PowerActive,
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
            Message = "Unable To Update Externals!"
        };
    }

    public async Task<ExternalResponseModel> GetById(int ExternalsId)
    {
        var Externals = await _externalRepo.Get(x => x.Id == ExternalsId && x.IsDeleted == false);

        if (Externals != null)
        {
            return new ExternalResponseModel()
            {
                Data = GetDetails(Externals),
                Status = true,
                Message = "Externals Retrieved Successfully!"
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
        var Externals = await _externalRepo.GetByExpression(x => x.IsDeleted == false);

        if (Externals != null)
        {
            return new ExternalsResponseModel()
            {
                Data = Externals.Select(x => GetDetails(x)).ToList(),
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
        var Externals = await _externalRepo.GetByExpression(x => x.RoomId == roomId && x.IsDeleted == false);

        if (Externals != null)
        {
            return new ExternalsResponseModel()
            {
                Data = Externals.Select(x => GetDetails(x)).ToList(),
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
        var Externals = await _externalRepo.GetByExpression(x => x.SectionId == sectionId && x.IsDeleted == false);

        if (Externals != null)
        {
            return new ExternalsResponseModel()
            {
                Data = Externals.Select(x => GetDetails(x)).ToList(),
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
        var Externals = await _externalRepo.Get(x => x.Id == ExternalsId);

        if (Externals != null)
        {
            Externals.IsActive = false;
            Externals.PowerActive = false;
            Externals.LastModifiedBy = personId;
            Externals.LastModifiedOn = DateTime.Now;
            Externals.DeletedOn = DateTime.Now;
            Externals.DeletedBy = personId;
            Externals.IsDeleted = true;

            await _externalRepo.Update(Externals);

            await _realtime.NotifyAll("ExternalsDeleted", new
            {
                Externals.Id,
                Externals.ExternalId
            });

            await _mqtt.PublishDeviceUpdate(Externals.ExternalId, new GenericDeviceUpdateState
            {
                FacilityType = FacilityType.External,
                Id = Externals.Id,
                BrightnessLevel = 0,
                IsActive = false,
                PowerActive = false,
                IsLocked = false,
                IsOpen = false
            });

            return new BaseResponse()
            {
                Status = true,
                Message = "Externals Deleted Successfully!"
            };
        }

        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Externals!"
        };
    }

    public GetExternalDto GetDetails(External External)
    {
        return new GetExternalDto()
        {
            Id = External.Id,
            ExternalsName = External.ExternalName,
            IsTriggered = External.IsTriggered,
            IsActive = External.IsActive,
            PowerActive = External.PowerActive,
            RoomId = External.RoomId,
            SectionId = External.SectionId,
            ExternalsId = External.ExternalId,
            LastModifiedBy = External.LastModifiedBy,
            DeletedBy = External.DeletedBy,
            CreatedBy = External.CreatedBy,
            LastModifiedOn = External.LastModifiedOn,
            CreatedOn = External.CreatedOn,
            IsDeleted = External.IsDeleted,
            IsHidden = External.IsHidden,
        };
    }
}