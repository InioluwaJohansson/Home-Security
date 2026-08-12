using Home_Security.Authentication;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Services;
public class MicroControllerService : IMicroControllerService
{
    IHardwareRepo _hardwareRepo;
    IApplianceRepo _applianceRepo;
    ILightRepo _lightRepo;
    ICameraRepo _cameraRepo;
    IDoorRepo _doorRepo;
    IWindowRepo _windowRepo;
    IExternalRepo _externalRepo;
    IActionService _actionService;
    IRealtimeNotificationService _realtime;
    IMqttService _mqttService;
    IJWTAuthentication _jWTAuthentication;
    IConfiguration _configuration;
    public MicroControllerService(IHardwareRepo hardwareRepo, IApplianceRepo applianceRepo, ILightRepo lightRepo, ICameraRepo cameraRepo, IDoorRepo doorRepo, IWindowRepo windowRepo, IExternalRepo externalRepo, IActionService actionService, IRealtimeNotificationService realtime, IMqttService mqttService, IJWTAuthentication jWTAuthentication, IConfiguration configuration)
    {
        _hardwareRepo = hardwareRepo;
        _applianceRepo = applianceRepo;
        _lightRepo = lightRepo;
        _cameraRepo = cameraRepo;
        _doorRepo = doorRepo;
        _windowRepo = windowRepo;
        _externalRepo = externalRepo;
        _actionService = actionService;
        _realtime = realtime;
        _mqttService = mqttService;
        _jWTAuthentication = jWTAuthentication;
        _configuration = configuration;
    }
    public async Task<MTTQConfig> GetHardwareMTQQAuth(string hardwareId, string AuthKey)
    {
        var hardware = await _hardwareRepo.Get(x => x.HardwareId == hardwareId && x.AuthKey == AuthKey && !x.IsDeleted && !x.IsActive);
        if (hardware != null)
        {
            return new MTTQConfig()
            {
                Host = _configuration["Mqtt:HostLenovo"] ?? _configuration["Mqtt:HostDell"] ?? _configuration["Mqtt:Host"],
                Port = _configuration["Mqtt:Port"],
                Username = _configuration["Mqtt:Username"],
                Password = _configuration["Mqtt:Password"],
                Status = true,
            };
        }
        return new MTTQConfig()
        {
            Status = false
        };
    }
    public async Task<GenericDeviceState> GetByAuth(string hardWareId, string AuthKey)
    {
        var hardware = await _hardwareRepo.Get(x => x.HardwareId == hardWareId && x.AuthKey == AuthKey && !x.IsDeleted && !x.IsActive);
        if (hardware != null)
        {
            hardware.IsActive = true;
            await _hardwareRepo.Update(hardware);
            var state = new GenericDeviceState()
            {
                ApplianceDeviceStates = await GetApplianceDeviceStates(hardware.ApplianceIds) ?? new List<ApplianceDeviceState>(),
                LightDeviceStates = await GetLightDeviceStates(hardware.LightIds) ?? new List<LightDeviceState>(),
                CameraDeviceStates = await GetCameraDeviceStates(hardware.CameraIds) ?? new List<CameraDeviceState>(),
                WindowDeviceStates = await GetWindowDeviceStates(hardware.WindowIds) ?? new List<WindowDeviceState>(),
                DoorDeviceStates = await GetDoorDeviceStates(hardware.DoorIds) ?? new List<DoorDeviceState>(),
                ExternalDeviceStates = await GetExternalDeviceStates(hardware.ExternalIds) ?? new List<ExternalDeviceState>(),
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Token = _jWTAuthentication.GenerateHardwareToken(hardware.HardwareId, hardware.AuthKey)
            };
            return state;
        }
        var emptyState = new GenericDeviceState()
        {
            ApplianceDeviceStates = new List<ApplianceDeviceState>(),
            LightDeviceStates = new List<LightDeviceState>(),
            CameraDeviceStates = new List<CameraDeviceState>(),
            WindowDeviceStates = new List<WindowDeviceState>(),
            DoorDeviceStates = new List<DoorDeviceState>(),
            ExternalDeviceStates = new List<ExternalDeviceState>(),
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };
        return emptyState;
    }
    public async Task SendUpdatedHardwareData()
    {
        var hardwares = await _hardwareRepo.GetByExpression(x => x.IsActive && !x.IsDeleted);
        if(hardwares != null)
        {
            foreach (var hardware in hardwares)
            {
                if (hardware != null)
                {
                    var state = new GenericDeviceState()
                    {
                        ApplianceDeviceStates = await GetApplianceDeviceStates(hardware.ApplianceIds) ?? new List<ApplianceDeviceState>(),
                        LightDeviceStates = await GetLightDeviceStates(hardware.LightIds) ?? new List<LightDeviceState>(),
                        CameraDeviceStates = await GetCameraDeviceStates(hardware.CameraIds) ?? new List<CameraDeviceState>(),
                        WindowDeviceStates = await GetWindowDeviceStates(hardware.WindowIds) ?? new List<WindowDeviceState>(),
                        DoorDeviceStates = await GetDoorDeviceStates(hardware.DoorIds) ?? new List<DoorDeviceState>(),
                        ExternalDeviceStates = await GetExternalDeviceStates(hardware.ExternalIds) ?? new List<ExternalDeviceState>(),
                        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                    };
                    await _mqttService.PublishFullStateSync(hardware.HardwareId, state);
                }
            }
        }
    }
    public async Task<BaseResponse> UpdateDeviceStates(GenericDeviceUpdateState genericDeviceUpdateState)
    {
        switch (genericDeviceUpdateState.FacilityType)
        {
            case FacilityType.Appliance:
                var appliance = await _applianceRepo.Get(x => x.Id == genericDeviceUpdateState.Id && x.IsDeleted == false);
                if (appliance != null)
                {
                    appliance.IsActive = genericDeviceUpdateState.IsActive;
                    await _applianceRepo.Update(appliance);
                    await _realtime.NotifyAll("ApplianceUpdated", new
                    {
                        appliance.Id,
                        appliance.IsActive
                    });
                    return new BaseResponse()
                    {
                        Status = true,
                        Message = "Appliance State Updated Successfully!"
                    };
                }
                break;
            case FacilityType.Light:
                var light = await _lightRepo.Get(x => x.Id == genericDeviceUpdateState.Id && x.IsDeleted == false);
                if (light != null)
                {
                    light.BrightnessLevel = genericDeviceUpdateState.BrightnessLevel;
                    light.IsActive = genericDeviceUpdateState.IsActive;
                    await _lightRepo.Update(light);
                    await _realtime.NotifyAll("LightUpdated", new
                    {
                        light.Id,
                        light.IsActive,
                        light.BrightnessLevel
                    });
                    return new BaseResponse()
                    {
                        Status = true,
                        Message = "Light State Updated Successfully!"
                    };
                }
                break;
            case FacilityType.Camera:
                var camera = await _cameraRepo.Get(x => x.Id == genericDeviceUpdateState.Id && x.IsDeleted == false);
                if (camera != null)
                {
                    camera.PowerActive = genericDeviceUpdateState.PowerActive;
                    await _cameraRepo.Update(camera);
                    await _realtime.NotifyAll("CameraUpdated", new
                    {
                        camera.Id,
                        camera.PowerActive
                    });
                    return new BaseResponse()
                    {
                        Status = true,
                        Message = "Camera State Updated Successfully!"
                    };
                }
                break;
            case FacilityType.Window:
                var window = await _windowRepo.Get(x => x.Id == genericDeviceUpdateState.Id && x.IsDeleted == false);
                if (window != null)
                {
                    window.IsOpen = genericDeviceUpdateState.IsOpen;
                    window.IsLocked = genericDeviceUpdateState.IsLocked;
                    await _windowRepo.Update(window);
                    await _realtime.NotifyAll("WindowUpdated", new
                    {
                        window.Id,
                        window.IsOpen,
                        window.IsLocked
                    });
                    return new BaseResponse()
                    {
                        Status = true,
                        Message = "Window State Updated Successfully!"
                    };
                }
                break;
            case FacilityType.Door:
                var door = await _doorRepo.Get(x => x.Id == genericDeviceUpdateState.Id && x.IsDeleted == false);
                if (door != null)
                {
                    door.IsOpen = genericDeviceUpdateState.IsOpen;
                    door.IsLocked = genericDeviceUpdateState.IsLocked;
                    await _doorRepo.Update(door);
                    await _realtime.NotifyAll("DoorUpdated", new
                    {
                        door.Id,
                        door.IsOpen,
                        door.IsLocked
                    });
                    return new BaseResponse()
                    {
                        Status = true,
                        Message = "Door State Updated Successfully!"
                    };
                }
                break;
            case FacilityType.External:
                var external = await _externalRepo.Get(x => x.Id == genericDeviceUpdateState.Id && x.IsDeleted == false);
                if (external != null)
                {
                    external.IsActive = genericDeviceUpdateState.IsActive;
                    await _externalRepo.Update(external);
                    if (genericDeviceUpdateState.IsTriggered == true) await _actionService.TriggerActionsForExternal(external.Id);
                    return new BaseResponse()
                    {
                        Status = true,
                        Message = "External State Updated Successfully!"
                    };
                }
                break;
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update Generic State!"
        };
    }
    private async Task <List<ApplianceDeviceState>> GetApplianceDeviceStates(List<int> ApplianceIds)
    {
        var applianceDeviceStates = new List<ApplianceDeviceState>();
        foreach (var item in ApplianceIds)
        {
            var appliance = await _applianceRepo.Get(x => x.Id == item && x.IsDeleted == false);
            if (appliance != null)
            {
                applianceDeviceStates.Add(new ApplianceDeviceState()
                {
                    ApplianceId = appliance.Id,
                    ApplianceType = appliance.ApplianceType,
                    IsActive = appliance.IsActive,
                });
            }
        }
        applianceDeviceStates.OrderBy(x => x.ApplianceId);
        return applianceDeviceStates;
    }
    private async Task <List<LightDeviceState>> GetLightDeviceStates(List<int> LightIds)
    {
        var lightDeviceStates = new List<LightDeviceState>();
        foreach (var item in LightIds)
        {
            var light = await _lightRepo.Get(x => x.Id == item && x.IsDeleted == false);
            if (light != null)
            {
                lightDeviceStates.Add(new LightDeviceState()
                {
                    LightId = light.Id,
                    BrightnessLevel = light.BrightnessLevel,
                    IsActive = light.IsActive,
                });
            }
        }
        lightDeviceStates.OrderBy(x => x.LightId);
        return lightDeviceStates;
    }
    private async Task <List<CameraDeviceState>> GetCameraDeviceStates(List<int> CameraIds)
    {
        var cameraDeviceStates = new List<CameraDeviceState>();
        foreach (var item in CameraIds)
        {
            var camera = await _cameraRepo.Get(x => x.Id == item && x.IsDeleted == false);
            if (camera != null)
            {
                cameraDeviceStates.Add(new CameraDeviceState()
                {
                    CameraId = camera.Id,
                    IsActive = camera.IsActive,
                });
            }
        }
        cameraDeviceStates.OrderBy(x => x.CameraId);
        return cameraDeviceStates;
    }
    private async Task <List<WindowDeviceState>> GetWindowDeviceStates(List<int> WindowIds)
    {
        var windowDeviceStates = new List<WindowDeviceState>();
        foreach (var item in WindowIds)
        {
            var window = await _windowRepo.Get(x => x.Id == item && x.IsDeleted == false);
            if (window != null)
            {
                windowDeviceStates.Add(new WindowDeviceState()
                {
                    WindowId = window.Id,
                    IsOpen = window.IsOpen,
                    IsLocked = window.IsLocked,
                });
            }
        }
        windowDeviceStates.OrderBy(x => x.WindowId);
        return windowDeviceStates;
    }
    private async Task <List<DoorDeviceState>> GetDoorDeviceStates(List<int> DoorIds)
    {
        var doorDeviceStates = new List<DoorDeviceState>();
        foreach (var item in DoorIds)
        {
            var door = await _doorRepo.Get(x => x.Id == item && x.IsDeleted == false);
            if (door != null)
            {
                doorDeviceStates.Add(new DoorDeviceState()
                {
                    DoorId = door.Id,
                    IsOpen = door.IsOpen,
                    IsLocked = door.IsLocked,
                    DoorType = door.DoorType
                });
            }
        }
        doorDeviceStates.OrderBy(x => x.DoorId);
        return doorDeviceStates;
    }
    private async Task <List<ExternalDeviceState>> GetExternalDeviceStates(List<int> ExternalIds)
    {
        var externalDeviceStates = new List<ExternalDeviceState>();
        foreach (var item in ExternalIds)
        {
            var external = await _externalRepo.Get(x => x.Id == item && x.IsDeleted == false);
            if (external != null)
            {
                externalDeviceStates.Add(new ExternalDeviceState()
                {
                    ExternalId = external.Id,
                    IsActive = external.IsActive,
                    IsTriggered = external.IsTriggered,
                });
            }
        }
        externalDeviceStates.OrderBy(x => x.ExternalId);
        return externalDeviceStates;
    }
}