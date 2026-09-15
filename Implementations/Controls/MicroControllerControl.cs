using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
namespace Home_Security.Implementations.Controls;
public class MicroControllerControl : IMicroControllerControl
{
    IAuthControl _authControl;
    IMicroControllerService _microControllerService;
    ILogService _logService;
    public MicroControllerControl(IAuthControl authControl, IMicroControllerService microControllerService, ILogService logService)
    {
        _authControl = authControl;
        _microControllerService = microControllerService;
        _logService = logService;
    }

    public async Task<GenericDeviceState> GetByAuth(string hardwareId, string AuthKey)
    {
        return await _microControllerService.GetByAuth(hardwareId, AuthKey);
    }

    public async Task<MTTQConfig> GetHardwareMTQQAuth(string hardwareId, string AuthKey)
    {
        return await _microControllerService.GetHardwareMTQQAuth(hardwareId, AuthKey);
    }
    public async Task<BaseResponse> UpdateDeviceStates(GetAuthControlInfoDto getAuthControlInfoDto, GenericDeviceUpdateState genericDeviceUpdateState)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.System)
        {
            var microController = await _microControllerService.UpdateDeviceStates(genericDeviceUpdateState, auth.Id);
            return microController;
        }
        return _authControl.AuthFaliure();
    }
}