using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface IMicroControllerService
{
    public Task<MTTQConfig> GetHardwareMTQQAuth(string hardwareId, string AuthKey);
    public Task<GenericDeviceState> GetByAuth(string hardWareId, string AuthKey);
    public Task<BaseResponse> UpdateDeviceStates(GenericDeviceUpdateState genericDeviceUpdateState, int personId);
    public Task SendUpdatedHardwareData();
}