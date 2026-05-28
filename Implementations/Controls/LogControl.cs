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
    public async Task<LogsResponseModel> GetAllLogsByPersonId(GetAuthControlInfoDto getAuthControlInfoDto, int personId, int page = 1, int pageSize = 50)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var log = await _logService.GetLogsByPersonId(personId, page, pageSize);
            return log;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
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
    public async Task<LogsResponseModel> GetAllLogsByDate(GetAuthControlInfoDto getAuthControlInfoDto, DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 50)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var log = await _logService.GetLogsByDate(startDate, endDate, page, pageSize);
            return log;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
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
    public async Task<LogsResponseModel> GetAllLogs(GetAuthControlInfoDto getAuthControlInfoDto, int page = 1, int pageSize = 50)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var log = await _logService.GetAllLogs(page, pageSize);
            return log;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
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