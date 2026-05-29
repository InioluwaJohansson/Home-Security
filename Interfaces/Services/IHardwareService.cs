using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Services;
public interface IHardwareService
{
    public Task<BaseResponse> CreateHardware(CreateHardwareDto createHardwareDto, int personId);
    public Task<BaseResponse> UpdateHardware(UpdateHardwareDto updateHardwareDto, int personId);
    public Task<HardwareResponseModel> GetById(int hardwareId);
    public Task<HardwaresResponseModel> GetAllHardwares();
    public Task CheckHardwareStatus();
}