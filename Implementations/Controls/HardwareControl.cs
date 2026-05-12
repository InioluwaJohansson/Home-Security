using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.Interfaces.Controls.Defaults;

namespace Home_Security.Implementations.Controls;

public class HardwareControl : IHardwareControl
{
    IHardwareService _hardwareService;
    IAuthControl _authControl;
    ILogService _logService;
    HardwareControl(IHardwareService hardwareService, IAuthControl authControl, ILogService logService)
    {
        _hardwareService = hardwareService;
        _authControl = authControl;
        _logService = logService;
    }
    public async Task<BaseResponse> CreateHardware(GetAuthControlInfoDto getAuthControlInfoDto, CreateHardwareDto createHardwareDto, int personId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var hardware = await _hardwareService.CreateHardware(createHardwareDto, personId);
            if (hardware.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "A Hardware was newly added. Awaiting Physical Interfacing!";
                await _logService.CreateLog(log);
            }
            return hardware;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateHardware(GetAuthControlInfoDto getAuthControlInfoDto, UpdateHardwareDto updateHardwareDto, int personId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var hardware = await _hardwareService.UpdateHardware(updateHardwareDto, personId);
            if (hardware.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = "A Hardware was updated. Awaiting Physical Interfacing!";
                await _logService.CreateLog(log);
            }
            return hardware;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> GetById(GetAuthControlInfoDto getAuthControlInfoDto, int hardwareId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var hardware = await _hardwareService.GetById(hardwareId);
            return hardware;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<HardwaresResponseModel> GetAllHardwares(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var hardware = await _hardwareService.GetAllHardwares();
            return hardware;
        }
        return new HardwaresResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
}
