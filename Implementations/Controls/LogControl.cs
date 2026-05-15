using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Implementations.Services;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Controls;
public class LogControl : ILogControl
{
    IAuthControl _authControl;
    ILogService _logService;
    public LogControl(IAuthControl authControl, ILogService logService)
    {
        _authControl = authControl;
        _logService = logService;
    }
    public async Task<LogResponseModel> GetLogById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var log = await _logService.GetLogById(id);
            return log;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new LogResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new LogResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<LogsResponseModel> GetAllLogsByPersonId(GetAuthControlInfoDto getAuthControlInfoDto, int personId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var log = await _logService.GetLogsByPersonId(personId);
            return log;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new LogsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new LogsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<LogsResponseModel> GetAllLogsByActionType(GetAuthControlInfoDto getAuthControlInfoDto, string actionType)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var log = await _logService.GetLogsByActionType(actionType);
            return log;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new LogsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new LogsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<LogsResponseModel> GetAllLogs(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var log = await _logService.GetAllLogs();
            return log;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new LogsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new LogsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
}