using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;

namespace Home_Security.Interfaces.Controls;
public interface IMicroControllerControl
{
    public Task<MTTQConfig> GetHardwareMTQQAuth(string hardwareId, string AuthKey);
    public Task<GenericDeviceState> GetByAuth(string hardWareId, string AuthKey);
    public Task<BaseResponse> UpdateDeviceStates(GetAuthControlInfoDto getAuthControlInfoDto, GenericDeviceUpdateState genericDeviceUpdateState);
}
