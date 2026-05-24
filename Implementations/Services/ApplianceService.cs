using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Services;

public class ApplianceService : IApplianceService
{
    IApplianceRepo _applianceRepo;
    IRealtimeNotificationService _realtime;
    IMqttService _mqtt;

    public ApplianceService(IApplianceRepo applianceRepo, IRealtimeNotificationService realtime, IMqttService mqtt)
    {
        _applianceRepo = applianceRepo;
        _realtime = realtime;
        _mqtt = mqtt;
    }

    public async Task<BaseResponse> CreateAppliance(CreateApplianceDto createApplianceDto, int personId)
    {
        if (createApplianceDto != null)
        {
            var appliance = new Appliance()
            {
                ApplianceId = $"APPLIANCE{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                ApplianceName = createApplianceDto.ApplianceName,
                IsActive = false,
                PowerActive = false,
                SectionId = createApplianceDto.SectionId,
                RoomId = createApplianceDto.RoomId,
                ApplianceType = createApplianceDto.ApplianceType,
                IsDeleted = false,
                CreatedBy = personId,
                LastModifiedBy = personId,
                LastModifiedOn = DateTime.Now,
                CreatedOn = DateTime.Now
            };

            await _applianceRepo.Create(appliance);

            await _realtime.NotifyAll("ApplianceCreated", GetDetails(appliance));

            await _mqtt.PublishDeviceUpdate(appliance.ApplianceId, new GenericDeviceUpdateState
            {
                FacilityType = FacilityType.Appliance,
                Id = appliance.Id,
                IsActive = appliance.IsActive,
                PowerActive = appliance.PowerActive,
                IsLocked = false,
                IsOpen = false,
                BrightnessLevel = 0
            });

            return new BaseResponse()
            {
                Status = true,
                Message = "Appliance Successfully Added!"
            };
        }

        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Add Appliance!"
        };
    }

    public async Task<BaseResponse> UpdateAppliance(UpdateApplianceDto updateApplianceDto, int personId)
    {
        if (updateApplianceDto != null)
        {
            var appliance = await _applianceRepo.Get(x => x.Id == updateApplianceDto.Id);

            appliance.ApplianceName = updateApplianceDto.ApplianceName ?? appliance.ApplianceName;
            appliance.SectionId = updateApplianceDto.SectionId != 0 ? updateApplianceDto.SectionId : appliance.SectionId;
            appliance.RoomId = updateApplianceDto.RoomId != 0 ? updateApplianceDto.RoomId : appliance.RoomId;
            appliance.IsActive = updateApplianceDto.IsActive;
            appliance.LastModifiedBy = personId;
            appliance.LastModifiedOn = DateTime.Now;
            appliance.ApplianceType = updateApplianceDto.ApplianceType;
            await _applianceRepo.Update(appliance);

            await _realtime.NotifyAll("ApplianceUpdated", GetDetails(appliance));

            await _mqtt.PublishDeviceUpdate(appliance.ApplianceId, new GenericDeviceUpdateState
            {
                FacilityType = FacilityType.Appliance,
                Id = appliance.Id,
                IsActive = appliance.IsActive,
                PowerActive = appliance.PowerActive,
                IsLocked = false,
                IsOpen = false,
                BrightnessLevel = 0
            });

            return new BaseResponse()
            {
                Status = true,
                Message = "Appliance Updated Successfully!"
            };
        }

        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update Appliance!"
        };
    }

    public async Task<ApplianceResponseModel> GetById(int applianceId)
    {
        var appliance = await _applianceRepo.Get(x => x.Id == applianceId && x.IsDeleted == false);

        if (appliance != null)
        {
            return new ApplianceResponseModel()
            {
                Data = GetDetails(appliance),
                Status = true,
                Message = "Appliance Retrieved Successfully!"
            };
        }

        return new ApplianceResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Appliance!"
        };
    }

    public async Task<AppliancesResponseModel> GetAllAppliances()
    {
        var appliances = await _applianceRepo.GetByExpression(x => x.IsDeleted == false);

        if (appliances != null)
        {
            return new AppliancesResponseModel()
            {
                Data = appliances.Select(x => GetDetails(x)).ToList(),
                Message = "Appliances Retrieved Successfully!",
                Status = true
            };
        }

        return new AppliancesResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Appliances!",
            Status = true
        };
    }

    public async Task<AppliancesResponseModel> GetAllAppliancesByRoomId(int roomId)
    {
        var appliances = await _applianceRepo.GetByExpression(x => x.RoomId == roomId && x.IsDeleted == false);

        if (appliances != null)
        {
            return new AppliancesResponseModel()
            {
                Data = appliances.Select(x => GetDetails(x)).ToList(),
                Message = "Appliances Retrieved Successfully!",
                Status = true
            };
        }

        return new AppliancesResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Appliances!",
            Status = true
        };
    }

    public async Task<AppliancesResponseModel> GetAllAppliancesBySectionId(int sectionId)
    {
        var appliances = await _applianceRepo.GetByExpression(x => x.SectionId == sectionId && x.IsDeleted == false);

        if (appliances != null)
        {
            return new AppliancesResponseModel()
            {
                Data = appliances.Select(x => GetDetails(x)).ToList(),
                Message = "Appliances Retrieved Successfully!",
                Status = true
            };
        }

        return new AppliancesResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Appliances!",
            Status = true
        };
    }

    public async Task<BaseResponse> Delete(int applianceId, int personId)
    {
        var appliance = await _applianceRepo.Get(x => x.Id == applianceId);

        if (appliance != null)
        {
            appliance.IsActive = false;
            appliance.PowerActive = false;
            appliance.LastModifiedBy = personId;
            appliance.LastModifiedOn = DateTime.Now;
            appliance.DeletedOn = DateTime.Now;
            appliance.DeletedBy = personId;

            await _applianceRepo.Update(appliance);

            await _realtime.NotifyAll("ApplianceDeleted", new
            {
                appliance.Id,
                appliance.ApplianceId
            });

            await _mqtt.PublishDeviceUpdate(appliance.ApplianceId, new GenericDeviceUpdateState
            {
                FacilityType = FacilityType.Appliance,
                Id = appliance.Id,
                IsActive = false,
                PowerActive = false,
                IsLocked = false,
                IsOpen = false,
                BrightnessLevel = 0
            });

            return new BaseResponse()
            {
                Status = true,
                Message = "Appliance Deleted Successfully!"
            };
        }

        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Appliance!"
        };
    }

    public GetApplianceDto GetDetails(Appliance appliance)
    {
        return new GetApplianceDto()
        {
            Id = appliance.Id,
            ApplianceName = appliance.ApplianceName,
            IsActive = appliance.IsActive,
            PowerActive = appliance.PowerActive,
            RoomId = appliance.RoomId,
            SectionId = appliance.SectionId,
            ApplianceId = appliance.ApplianceId,
            ApplianceType = appliance.ApplianceType,
            LastModifiedBy = appliance.LastModifiedBy,
            DeletedBy = appliance.DeletedBy,
            CreatedBy = appliance.CreatedBy,
            LastModifiedOn = appliance.LastModifiedOn,
            CreatedOn = appliance.CreatedOn,
            IsDeleted = appliance.IsDeleted,
        };
    }
}