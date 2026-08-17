using Home_Security.Authentication;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Services;
public class MicroControllerService : IMicroControllerService
{
    IHardwareRepo _hardwareRepo;
    IApplianceService _applianceService;
    ILightService _lightService;
    ICameraService _cameraService;
    IDoorService _doorService;
    IWindowService _windowService;
    IExternalService _externalService;
    IActionService _actionService;
    IMqttService _mqttService;
    IJWTAuthentication _jWTAuthentication;
    IConfiguration _configuration;
    public MicroControllerService(IHardwareRepo hardwareRepo, IApplianceService applianceService, ILightService lightService, ICameraService cameraService, IDoorService doorService, IWindowService windowService, IExternalService externalService, IActionService actionService, IMqttService mqttService, IJWTAuthentication jWTAuthentication, IConfiguration configuration)
    {
        _hardwareRepo = hardwareRepo;
        _applianceService = applianceService;
        _lightService = lightService;
        _cameraService = cameraService;
        _doorService = doorService;
        _windowService = windowService;
        _externalService = externalService;
        _actionService = actionService;
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
                Data = new MTTQData()
                {
                    Host = _configuration["Mqtt:HostLenovo"] ?? _configuration["Mqtt:HostDell"] ?? _configuration["Mqtt:Host"],
                    Port = _configuration["Mqtt:Port"],
                    Username = _configuration["Mqtt:Username"],
                    Password = _configuration["Mqtt:Password"],
                },
                Status = true,
                Message = "Data Retrieved"
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
    public async Task<BaseResponse> UpdateDeviceStates(GenericDeviceUpdateState genericDeviceUpdateState, int personId)
    {
        switch (genericDeviceUpdateState.FacilityType)
        {
            case FacilityType.Appliance:
                var appliance = new UpdateApplianceDto()
                {
                    IsActive = genericDeviceUpdateState.IsActive,
                    Id = genericDeviceUpdateState.Id
                };
                await _applianceService.UpdateAppliance(appliance, personId);
                break;
            case FacilityType.Light:
                var light = new UpdateLightDto()
                {
                    Id = genericDeviceUpdateState.Id,
                    BrightnessLevel = genericDeviceUpdateState.BrightnessLevel,
                    IsActive = genericDeviceUpdateState.IsActive,
                };
                await _lightService.UpdateLight(light, personId);
                break;
            case FacilityType.Camera:
                var camera = new UpdateCameraDto()
                {
                    Id = genericDeviceUpdateState.Id,
                    IsActive = genericDeviceUpdateState.IsActive,
                };
                    await _cameraService.UpdateCamera(camera, personId);
                break;
            case FacilityType.Window:
                var window = new UpdateWindowDto()
                {
                    Id = genericDeviceUpdateState.Id,
                    IsOpen = genericDeviceUpdateState.IsOpen,
                    IsLocked = genericDeviceUpdateState.IsLocked,
                };
                await _windowService.UpdateWindow(window, personId);
                break;
            case FacilityType.Door:
                var door = new UpdateDoorDto()
                {
                    Id = genericDeviceUpdateState.Id,
                    IsOpen = genericDeviceUpdateState.IsOpen,
                    IsLocked = genericDeviceUpdateState.IsLocked
                };
                    await _doorService.UpdateDoor(door, personId);
                break;
            case FacilityType.External:
                var external = new UpdateExternalDto()
                {
                    Id = genericDeviceUpdateState.Id,
                    IsActive = genericDeviceUpdateState.IsActive,
                };
                await _externalService.UpdateExternal(external, personId);
                if (genericDeviceUpdateState.IsTriggered == true) await _actionService.TriggerActionsForExternal(external.Id);
                break;
        }
        return new BaseResponse()
        {
            Status = true,
            Message = "Generic State Updated!"
        };
    }
    private async Task <List<ApplianceDeviceState>> GetApplianceDeviceStates(List<int> ApplianceIds)
    {
        var applianceDeviceStates = new List<ApplianceDeviceState>();
        foreach (var item in ApplianceIds)
        {
            var appliance = await _applianceService.GetById(item);
            if (appliance.Data != null)
            {
                applianceDeviceStates.Add(new ApplianceDeviceState()
                {
                    ApplianceId = appliance.Data.Id,
                    ApplianceType = Enum.Parse<ApplianceType>(appliance.Data.ApplianceType),
                    IsActive = appliance.Data.IsActive,
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
            var light = await _lightService.GetById(item);
            if (light != null)
            {
                lightDeviceStates.Add(new LightDeviceState()
                {
                    LightId = light.Data.Id,
                    BrightnessLevel = light.Data.BrightnessLevel,
                    IsActive = light.Data.IsActive,
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
            var camera = await _cameraService.GetById(item);
            if (camera != null)
            {
                cameraDeviceStates.Add(new CameraDeviceState()
                {
                    CameraId = camera.Data.Id,
                    IsActive = camera.Data.IsActive,
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
            var window = await _windowService.GetById(item);
            if (window != null)
            {
                windowDeviceStates.Add(new WindowDeviceState()
                {
                    WindowId = window.Data.Id,
                    IsOpen = window.Data.IsOpen,
                    IsLocked = window.Data.IsLocked,
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
            var door = await _doorService.GetById(item);
            if (door != null)
            {
                doorDeviceStates.Add(new DoorDeviceState()
                {
                    DoorId = door.Data.Id,
                    IsOpen = door.Data.IsOpen,
                    IsLocked = door.Data.IsLocked,
                    DoorType = door.Data.DoorTypeId
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
            var external = await _externalService.GetById(item);
            if (external != null)
            {
                externalDeviceStates.Add(new ExternalDeviceState()
                {
                    ExternalId = external.Data.Id,
                    IsActive = external.Data.IsActive,
                    IsTriggered = external.Data.IsTriggered,
                });
            }
        }
        externalDeviceStates.OrderBy(x => x.ExternalId);
        return externalDeviceStates;
    }
}