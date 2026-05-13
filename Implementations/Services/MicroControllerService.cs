using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
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
    ILogRepo _logRepo;

    RealtimeNotificationService _realtime;

    public MicroControllerService(
        IHardwareRepo hardwareRepo,
        IApplianceRepo applianceRepo,
        ILightRepo lightRepo,
        ICameraRepo cameraRepo,
        IDoorRepo doorRepo,
        IWindowRepo windowRepo,
        ILogRepo logRepo,
        RealtimeNotificationService realtime)
    {
        _hardwareRepo = hardwareRepo;
        _applianceRepo = applianceRepo;
        _lightRepo = lightRepo;
        _cameraRepo = cameraRepo;
        _doorRepo = doorRepo;
        _windowRepo = windowRepo;
        _logRepo = logRepo;
        _realtime = realtime;
    }

    public async Task<GenericDeviceState> GetByAuth(string hardWareId, string AuthKey)
    {
        var hardware = await _hardwareRepo.Get(x => x.HardwareId == hardWareId && x.AuthKey == AuthKey && x.IsDeleted == false);

        if (hardware != null)
        {
            var state = new GenericDeviceState()
            {
                ApplianceDeviceStates = await GetApplianceDeviceStates(hardware.ApplianceIds),
                LightDeviceStates = await GetLightDeviceStates(hardware.LightIds),
                CameraDeviceStates = await GetCameraDeviceStates(hardware.CameraIds),
                WindowDeviceStates = await GetWindowDeviceStates(hardware.WindowIds),
                DoorDeviceStates = await GetDoorDeviceStates(hardware.DoorIds),
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            // ✅ SIGNALR ADDED (device sync broadcast)
            await _realtime.NotifyAll("DeviceStateSnapshot", state);

            return state;
        }

        var emptyState = new GenericDeviceState()
        {
            ApplianceDeviceStates = new List<ApplianceDeviceState>(),
            LightDeviceStates = new List<LightDeviceState>(),
            CameraDeviceStates = new List<CameraDeviceState>(),
            WindowDeviceStates = new List<WindowDeviceState>(),
            DoorDeviceStates = new List<DoorDeviceState>(),
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        return emptyState;
    }

    public async Task<BaseResponse> UpdateDeviceStates(GenericDeviceUpdateState genericDeviceUpdateState)
    {
        switch (genericDeviceUpdateState.FacilityType)
        {
            case 1:
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

            case 2:
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

            case 3:
                var camera = await _cameraRepo.Get(x => x.Id == genericDeviceUpdateState.Id && x.IsDeleted == false);
                if (camera != null)
                {
                    camera.IsActive = genericDeviceUpdateState.IsActive;
                    await _cameraRepo.Update(camera);
                    await _realtime.NotifyAll("CameraUpdated", new
                    {
                        camera.Id,
                        camera.IsActive
                    });
                    return new BaseResponse()
                    {
                        Status = true,
                        Message = "Camera State Updated Successfully!"
                    };
                }
                break;

            case 4:
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

            case 5:
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
                    IsActive = appliance.IsActive,
                });
            }
        }
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
                });
            }
        }
        return doorDeviceStates;
    }
}