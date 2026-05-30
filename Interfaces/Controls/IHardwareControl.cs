using Home_Security.Entities.Identity;
using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Controls;

public interface IHardwareControl
{
    public Task<BaseResponse> CreateHardware(GetAuthControlInfoDto getAuthControlInfoDto, CreateHardwareDto createHardwareDto, int personId);
    public Task<BaseResponse> UpdateHardware(GetAuthControlInfoDto getAuthControlInfoDto, UpdateHardwareDto updateHardwareDto, int personId);
    public Task<BaseResponse> GetById(GetAuthControlInfoDto getAuthControlInfoDto, int hardwareId);
    public Task<HardwaresResponseModel> GetAllHardwares(GetAuthControlInfoDto getAuthControlInfoDto);
}