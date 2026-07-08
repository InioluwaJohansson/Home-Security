using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Services;

public class LightService : ILightService
{
    ILightRepo _lightRepo;
    IHardwareService _hardwareService;
    IRealtimeNotificationService _realtime;
    IMqttService _mqtt;

    public LightService(ILightRepo lightRepo, IHardwareService hardwareService, IRealtimeNotificationService realtime, IMqttService mqtt)
    {
        _lightRepo = lightRepo;
        _hardwareService = hardwareService;
        _realtime = realtime;
        _mqtt = mqtt;
    }

    public async Task<BaseResponse> CreateLight(CreateLightDto createLightDto, int personId)
    {
        if (createLightDto != null)
        {
            var light = new Light()
            {
                LightId = $"LIGHT{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                LightName = createLightDto.LightName,
                SectionId = createLightDto.SectionId ?? null,
                RoomId = createLightDto.RoomId ?? null,
                IsDeleted = false,
                CreatedBy = personId,
                LastModifiedBy = personId,
                LastModifiedOn = DateTime.Now,
                CreatedOn = DateTime.Now,
                BrightnessLevel = 0,
            };

            await _lightRepo.Create(light);
            var lightData = GetDetails(await _lightRepo.Get(x => x.LightId == light.LightId));
            if(lightData.IsHidden){ await _realtime.NotifyAll("LightCreated", new List<Role> { Role.Owner, Role.Wife }, lightData);}
            else await _realtime.NotifyAll("LightCreated", lightData);
            return new BaseResponse()
            {
                Status = true,
                Message = "Light Successfully Added!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Add Light!"
        };
    }

    public async Task<BaseResponse> UpdateLight(UpdateLightDto updateLightDto, int personId)
    {
        if (updateLightDto != null)
        {
            var light = await _lightRepo.Get(x => x.Id == updateLightDto.Id);

            light.LightName = updateLightDto.LightName ?? light.LightName;
            light.LastModifiedBy = personId;
            light.LastModifiedOn = DateTime.Now;
            light.BrightnessLevel = updateLightDto.BrightnessLevel;
            light.IsActive = updateLightDto.IsActive;
            light.SectionId = updateLightDto.SectionId ?? null;
            light.RoomId = updateLightDto.RoomId ?? null;
            await _lightRepo.Update(light);
            if(light.IsHidden){ await _realtime.NotifyAll("LightUpdated", new List<Role> { Role.Owner, Role.Wife }, GetDetails(light));}
            else await _realtime.NotifyAll("LightUpdated", GetDetails(light));
            var hardware = await _hardwareService.GetHardwareName(light.Id, FacilityType.Light);
            await _mqtt.PublishDeviceUpdate(light.LightId, new GenericDeviceUpdateState
            {
                FacilityType = FacilityType.Light,
                Id = light.Id,
                BrightnessLevel = light.BrightnessLevel,
                IsActive = light.IsActive,
                PowerActive = light.PowerActive,
                IsLocked = false,
                IsOpen = false
            });

            return new BaseResponse()
            {
                Status = true,
                Message = "Light Updated Successfully!"
            };
        }

        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update Light!"
        };
    }

    public async Task<LightResponseModel> GetById(int lightId)
    {
        var light = await _lightRepo.Get(x => x.Id == lightId && x.IsDeleted == false);

        if (light != null)
        {
            return new LightResponseModel()
            {
                Data = GetDetails(light),
                Status = true,
                Message = "Light Retrieved Successfully!"
            };
        }

        return new LightResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Light!"
        };
    }

    public async Task<LightsResponseModel> GetAllLights()
    {
        var lights = await _lightRepo.GetByExpression(x => x.IsDeleted == false);

        if (lights != null)
        {
            return new LightsResponseModel()
            {
                Data = lights.Select(x => GetDetails(x)).ToList(),
                Message = "Lights Retrieved Successfully!",
                Status = true
            };
        }

        return new LightsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Lights!",
            Status = false
        };
    }

    public async Task<LightsResponseModel> GetAllLightsByRoomId(int roomId)
    {
        var lights = await _lightRepo.GetByExpression(x => x.RoomId == roomId && x.IsDeleted == false);

        if (lights != null)
        {
            return new LightsResponseModel()
            {
                Data = lights.Select(x => GetDetails(x)).ToList(),
                Message = "Lights Retrieved Successfully!",
                Status = true
            };
        }

        return new LightsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Lights!",
            Status = false
        };
    }

    public async Task<LightsResponseModel> GetAllLightsBySectionId(int sectionId)
    {
        var lights = await _lightRepo.GetByExpression(x => x.SectionId == sectionId && x.IsDeleted == false);

        if (lights != null)
        {
            return new LightsResponseModel()
            {
                Data = lights.Select(x => GetDetails(x)).ToList(),
                Message = "Lights Retrieved Successfully!",
                Status = true
            };
        }

        return new LightsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Lights!",
            Status = false
        };
    }

    public async Task<BaseResponse> Delete(int lightId, int personId)
    {
        var light = await _lightRepo.Get(x => x.Id == lightId);

        if (light != null)
        {
            light.IsActive = false;
            light.PowerActive = false;
            light.LastModifiedBy = personId;
            light.LastModifiedOn = DateTime.Now;
            light.DeletedOn = DateTime.Now;
            light.DeletedBy = personId;
            light.IsDeleted = true;

            await _lightRepo.Update(light);

            await _realtime.NotifyAll("LightDeleted", new
            {
                light.Id,
            });

            await _mqtt.PublishDeviceUpdate(light.LightId, new GenericDeviceUpdateState
            {
                FacilityType = FacilityType.Light,
                Id = light.Id,
                BrightnessLevel = 0,
                IsActive = false,
                PowerActive = false,
                IsLocked = false,
                IsOpen = false
            });

            return new BaseResponse()
            {
                Status = true,
                Message = "Light Deleted Successfully!"
            };
        }

        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Light!"
        };
    }

    public GetLightDto GetDetails(Light light)
    {
        return new GetLightDto()
        {
            Id = light.Id,
            LightName = light.LightName,
            IsActive = light.IsActive,
            PowerActive = light.PowerActive,
            RoomId = light.RoomId,
            SectionId = light.SectionId,
            LightId = light.LightId,
            LastModifiedBy = light.LastModifiedBy,
            DeletedBy = light.DeletedBy,
            CreatedBy = light.CreatedBy,
            LastModifiedOn = light.LastModifiedOn,
            CreatedOn = light.CreatedOn,
            IsDeleted = light.IsDeleted,
            BrightnessLevel = light.BrightnessLevel,
            IsHidden = light.IsHidden,
        };
    }
}