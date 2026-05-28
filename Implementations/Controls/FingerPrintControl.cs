using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Controls;
public class FingerPrintControl : IFingerPrintControl
{
    IAuthControl _authControl;
    IFingerPrintService _fingerPrintService;
    ILogService _logService;
    public FingerPrintControl(IAuthControl authControl, IFingerPrintService fingerPrintService, ILogService logService)
    {
        _authControl = authControl;
        _fingerPrintService = fingerPrintService;
        _logService = logService;
    }
    public async Task<BaseResponse> CreateFingerPrint(GetAuthControlInfoDto getAuthControlInfoDto, CreateFingerPrintDto createFingerPrintDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var fingerPrint = await _fingerPrintService.CreateFingerPrint(createFingerPrintDto, auth.Id);
            if (fingerPrint.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "An FingerPrint was newly added. Awaiting Physical Interfacing!";
                await _logService.CreateLog(log);
            }
            return fingerPrint;
        }
        return _authControl.AuthFaliure();
    }
}