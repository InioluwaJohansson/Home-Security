using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Services;
public class HardwareService : IHardwareService
{
    IHardwareRepo _hardwareRepo;
    public HardwareService(IHardwareRepo hardwareRepo)
    {
        _hardwareRepo = hardwareRepo;
    }
    public async Task<BaseResponse> CreateHardware(CreateHardwareDto createHardwareDto, int personId)
    {
        throw new NotImplementedException();
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
    public Task<HardwaresResponseModel> GetAllHardwares()
    {
        throw new NotImplementedException();
    }
    public GetHardwareDto GetDetails(Hardware hardware)
    {
        return new GetHardwareDto()
        {
            Id = hardware.Id,
            HardwareName = hardware.HardwareName,
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
}