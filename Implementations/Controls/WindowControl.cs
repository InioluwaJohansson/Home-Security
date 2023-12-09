using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Implementations.Services;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Controls.Defaults;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;

namespace Home_Security.Implementations.Controls;
public class WindowControl : IWindowControl
{
    IAuthControl _authControl;
    IWindowService _windowService;
    ILogService _logService;
    IObjectDefault _objectDefault;
    public WindowControl(IAuthControl authControl, IWindowService windowService, ILogService logService, IObjectDefault objectDefault)
    {
        _authControl = authControl;
        _windowService = windowService;
        _logService = logService;
        _objectDefault = objectDefault;
    }
    public async Task<BaseResponse> CreateWindow(GetAuthControlInfoDto getAuthControlInfoDto, CreateWindowDto createWindowDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            createWindowDto.personId = auth.Id;
            var window = await _windowService.CreateWindow(createWindowDto);
            if(window.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "A Window was newly added. Awaiting Physical Interfacing!";
                await _logService.CreateLog(log);
                return window;
            }
            
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateWindow(GetAuthControlInfoDto getAuthControlInfoDto, UpdateWindowDto updateWindowDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            updateWindowDto.personId = auth.Id;
            var window = await _windowService.UpdateWindow(updateWindowDto);
            if (window.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateWindowDto.WindowName} Window was recently updated!";
                await _logService.CreateLog(log);
            }
            return window;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UnlockWindow(GetAuthControlInfoDto getAuthControlInfoDto, UpdateWindowDto updateWindowDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var fail = _authControl.AuthFaliure();
            var getWindow = await _windowService.GetById(updateWindowDto.Id);
            if (getWindow != null && getWindow.Data.IsLocked == true)
            {
                var personCheck = await _authControl.GetPersonDetails(getWindow.Data.LockedBy);
                if (_authControl.CompareRole(personCheck.Role, auth.Role))
                {
                    updateWindowDto.personId = auth.Id;
                    updateWindowDto.UnlockedBy = auth.Id;
                    updateWindowDto.IsLocked = false;
                    var window = await _windowService.UpdateWindow(updateWindowDto);
                    if (window.Status == true)
                    {
                        var log = _authControl.CreateLog(auth.Id);
                        log.ActionType = "Unlocke";
                        log.LogDetails = $"{updateWindowDto.WindowName}  Window was locked!";
                        await _logService.CreateLog(log);
                    }
                    return window;
                }
                fail.Message = "Persmission Required";
                return fail;
            }
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> LockWindow(GetAuthControlInfoDto getAuthControlInfoDto, UpdateWindowDto updateWindowDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            updateWindowDto.personId = auth.Id;
            updateWindowDto.IsOpen = false;
            updateWindowDto.LockedBy = auth.Id;
            updateWindowDto.IsLocked = true;
            var window = await _windowService.UpdateWindow(updateWindowDto);
            if (window.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Locked";
                log.LogDetails = $"{updateWindowDto.WindowName} Window was Locked!";
                await _logService.CreateLog(log);
            }
            return window;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<WindowResponseModel> GetWindowById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var window = await _windowService.GetById(id);
            return window;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new WindowResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new WindowResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<WindowsResponseModel> GetAllWindowsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int windowId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var window = await _windowService.GetAllWindowsByRoomId(windowId);
            return window;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new WindowsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new WindowsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<WindowsResponseModel> GetAllWindowsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var window = await _windowService.GetAllWindowsBySectionId(sectionId);
            return window;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new WindowsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new WindowsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<WindowsResponseModel> GetAllWindows(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var window = await _windowService.GetAllWindows();
            return window;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new WindowsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new WindowsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<BaseResponse> DeleteWindow(GetAuthControlInfoDto getAuthControlInfoDto, int windowId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var window = await _windowService.Delete(windowId, auth.Id);
            if(window.Status == true)
            {
                var getWindow = await _objectDefault.WindowName(windowId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getWindow} Window was Deleted!";
                await _logService.CreateLog(log);
                return window;
            }
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
}