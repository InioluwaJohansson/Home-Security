using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Interfaces.Controls;
using Home_Security.RealTimeServices;
using Home_Security.Models.Enums;
namespace Home_Security.Implementations.Services;
public class HardwareService : IHardwareService
{
    IHardwareRepo _hardwareRepo;
    IRealtimeNotificationService _realtime;
    IAuthControl _authControl;
    public HardwareService(IHardwareRepo hardwareRepo, IRealtimeNotificationService realtime, IAuthControl authControl)
    {
        _hardwareRepo = hardwareRepo;
        _realtime = realtime;
        _authControl = authControl;
    }
    public async Task<BaseResponse> CreateHardware(CreateHardwareDto createHardwareDto, int personId)
        {
        if (createHardwareDto != null)
        {
            var hardware = new Hardware()
            {
                HardwareId = $"HARDWARE{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                AuthKey = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 18).ToUpper(),
                HardwareName = createHardwareDto.HardwareName,
                CreatedOn = DateTime.Now,
                CreatedBy = personId,
                LastModifiedBy = personId,
                LastModifiedOn = DateTime.Now,
                IsDeleted = false,
            };
            await _hardwareRepo.Create(hardware);
            var hardwareData = await _hardwareRepo.Get(x => x.HardwareId == hardware.HardwareId);
            await _realtime.NotifyAll("HardwareCreated", GetDetails(hardwareData));
            return new BaseResponse()
            {
                Status = true,
                Message = "Hardware Added Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Add Hardware!"
        };
    }
    public async Task<BaseResponse> UpdateHardware(UpdateHardwareDto updateHardwareDto, int personId)
    {
        if (updateHardwareDto != null)
        {
            var hardware = await _hardwareRepo.Get(x => x.Id == updateHardwareDto.Id);
            hardware.HardwareName = updateHardwareDto.HardwareName ?? hardware.HardwareName;
            hardware.CameraIds = updateHardwareDto.CameraIds ?? hardware.CameraIds;
            hardware.LightIds = updateHardwareDto.LightIds ?? hardware.LightIds;
            hardware.WindowIds = updateHardwareDto.WindowIds ?? hardware.WindowIds;
            hardware.DoorIds = updateHardwareDto.DoorIds ?? hardware.DoorIds;
            hardware.ExternalIds = updateHardwareDto.ExternalIds ?? hardware.ExternalIds;
            hardware.LastModifiedBy = personId;
            hardware.LastModifiedOn = DateTime.Now;
            await _hardwareRepo.Update(hardware);
            await _realtime.NotifyAll("HardwareUpdated", GetDetails(hardware));
            return new BaseResponse()
            {
                Status = true,
                Message = "hardware Updated Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update hardware!"
        };
    }
    public async Task<HardwareResponseModel> GetById(int hardwareId)
    {
        var hardware = await _hardwareRepo.Get(x => x.Id == hardwareId && x.IsDeleted == false);
        if(hardware != null)
        {
            return new HardwareResponseModel()
            {
                Data = GetDetails(hardware),
                Status = true,
                Message = "Hardware Retrieved Successfully!"
            };
        }
        return new HardwareResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Hardware!"
        };
    }
    public async Task<HardwaresResponseModel> GetAllHardwares()
    {
        var hardwares = await _hardwareRepo.GetByExpression(x => x.IsDeleted == false);
        if (hardwares != null)
        {
            return new HardwaresResponseModel()
            {
                Data = hardwares.Select(x => GetDetails(x)).ToList(),
                Message = "Hardwares Retrieved Successfully!",
                Status = true
            };
        }
        return new HardwaresResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Hardwares!",
            Status = true
        };
    }
    public async Task CheckHardwareStatus()
    {
        var hardwares = await _hardwareRepo.GetByExpression(x => x.IsDeleted == false && x.PowerActive == true);
        if (hardwares != null)
        {
            foreach (var hardware in hardwares)
            {
                if (hardware.LastModifiedOn < DateTime.Now.AddMinutes(-5))
                {
                    hardware.PowerActive = false;
                    await _hardwareRepo.Update(hardware);
                    await _realtime.NotifyAll("HardwareUpdated", GetDetails(hardware));
                }
            }
        }
    }
    public async Task<string> GetHardwareName(int id, FacilityType facilityType)
    {
        var hardware = await _hardwareRepo.GetHardwareByFacilityTypeId(id, facilityType);
        if(hardware != null) return hardware.HardwareId;
        return null;
    }
    public GetHardwareDto GetDetails(Hardware hardware)
    {
        return new GetHardwareDto()
        {
            Id = hardware.Id,
            Name = hardware.HardwareName,
            AuthKey = hardware.AuthKey,
            IsActive = hardware.IsActive,
            PowerActive = hardware.PowerActive,
            HardwareId = hardware.HardwareId,
            ApplianceIdNames = hardware.ApplianceIds.Select(x => GenericIdDetails(x, FacilityType.Appliance)).ToList(),
            LightIdNames = hardware.LightIds.Select(x => GenericIdDetails(x, FacilityType.Light)).ToList(),
            CameraIdNames = hardware.CameraIds.Select(x => GenericIdDetails(x, FacilityType.Camera)).ToList(),
            WindowIdNames = hardware.WindowIds.Select(x => GenericIdDetails(x, FacilityType.Window)).ToList(),
            DoorIdNames = hardware.DoorIds.Select(x => GenericIdDetails(x, FacilityType.Door)).ToList(),
            ExternalIdNames = hardware.ExternalIds.Select(x => GenericIdDetails(x, FacilityType.External)).ToList()
        };
    }
    private GenericIdNames GenericIdDetails(int id, FacilityType facilityType)
    {
        switch (facilityType)
        {
            case FacilityType.Appliance:
                string data = _authControl.GetApplianceName(id);
                if(data != null){
                    return new GenericIdNames(){
                        Id = id,
                        Name = data
                    };
                }
                break;
            case FacilityType.Light:
                string light = _authControl.GetLightName(id);
                if(light != null){
                    return new GenericIdNames(){
                        Id = id,
                        Name = light
                    };
                }
                break;
            case FacilityType.Camera:
                string camera = _authControl.GetCameraName(id);
                if(camera != null){
                    return new GenericIdNames(){
                        Id = id,
                        Name = camera
                    };
                }
                break;
            case FacilityType.External:
                string external = _authControl.GetExternalName(id);
                if(external != null){
                    return new GenericIdNames(){
                        Id = id,
                        Name = external
                    };
                }
                break;
            case FacilityType.Door:
                string door = _authControl.GetDoorName(id);
                if(door != null){
                    return new GenericIdNames(){
                        Id = id,
                        Name = door
                    };
                }
                break;
            case FacilityType.Window:
                string window = _authControl.GetWindowName(id);
                if(window != null){
                    return new GenericIdNames(){
                        Id = id,
                        Name = window
                    };
                }
                break;
        }
        return new GenericIdNames(){
            Id = id,
            Name = null
        };
    }
}