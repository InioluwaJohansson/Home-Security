using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Interfaces.Controls;
namespace Home_Security.Implementations.Services;
public class HardwareService : IHardwareService
{
    IHardwareRepo _hardwareRepo;
    IAuthControl _authControl;
    public HardwareService(IHardwareRepo hardwareRepo, IAuthControl authControl;)
    {
        _hardwareRepo = hardwareRepo;
        _authControl = auth control;
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
            hardware.LastModifiedBy = personId;
            hardware.LastModifiedOn = DateTime.Now;
            await _hardwareRepo.Update(hardware);
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
    public async Task<HardwareIdsResponseModel> GetById(string hardWareId, string AuthKey)
    {
        var hardware = await _hardwareRepo.Get(x => x.HardwareId == hardWareId && x.Auth == AuthKey && x.IsDeleted == false);
        if(hardware != null)
        {
            return new HardwareIdsResponseModel()
            {
                Data = GetIdsDetails(hardware),
                Status = true,
                Message = "Hardware Retrieved Successfully!"
            };
        }
        return new HardwareIdsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Hardware!"
        };
    }
    public Task<HardwaresResponseModel> GetAllHardwares()
    {
        var hardwares = await _hardwareRepo.GetByExpression(x => x.IsDeleted == false);
        if (hardwares != null)
        {
            return new HardwaresResponseModel()
            {
                Data = appliances.Select(x => GetDetails(x)).ToList(),
                Message = "Appliances Retrieved Successfully!",
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
    public GetHardwareDto GetDetails(Hardware hardware)
    {
        return new GetHardwareDto()
        {
            Id = hardware.Id,
            HardwareName = hardware.HardwareName,
AuthKey = hardware.AuthKey,
            IsActive = hardware.IsActive,
            PowerActive = hardware.PowerActive,
            RoomId = hardware.RoomId,
            SectionId = hardware.SectionId,
            hardwareId = hardware.hardwareId,
            LastModifiedBy = hardware.LastModifiedBy,
            DeletedBy = hardware.DeletedBy,
            CreatedBy = hardware.CreatedBy,
            LastModifiedOn = hardware.LastModifiedOn,
            CreatedOn = hardware.CreatedOn,
            IsDeleted = hardware.IsDeleted,
        };
    }
public GetHardwareIdsDto GetIdsDetails(Hardware hardware)
    {
        return new GetHardwareDto()
        {
            Id = hardware.Id,
   ApplianceIds= hardware.ApplianceIds,
   CameraIds = hardware.CameraIds,
   LightIds = hardware.LightsIds,
   WindowIds = hardware.WindowIds,
   DoorIds = hardware.DoorIds,
        };
    }
}