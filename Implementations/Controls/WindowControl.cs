using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Implementations.Services;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;

namespace Home_Security.Implementations.Controls;
public class WindowControl : IWindowControl
{
    IAuthControl _authControl;
    IWindowService _windowService;
    ILogService _logService;
    public WindowControl(IAuthControl authControl, IWindowService windowService, ILogService logService)
    {
        _authControl = authControl;
        _windowService = windowService;
        _logService = logService;
    }
    public async Task<BaseResponse> CreateWindow(GetAuthControlInfoDto getAuthControlInfoDto, CreateWindowDto createWindowDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var window = await _windowService.CreateWindow(createWindowDto, auth.Id);
            if(window.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "A Window was newly added. Awaiting Physical Interfacing!";
                log.FacilityType = FacilityTypeApp.Window;
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
            var window = await _windowService.UpdateWindow(updateWindowDto, auth.Id);
            if (window.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateWindowDto.WindowName} Window was recently updated!";
                log.FacilityType = FacilityTypeApp.Window;
                log.FacilityId = updateWindowDto.Id;
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
        var log = _authControl.CreateLog(auth.Id);
        if (auth.Status != false)
        {
            var getWindow = await _windowService.GetById(updateWindowDto.Id);
            if(getWindow != null && getWindow.Data.IsOpen == true){  
                updateWindowDto.IsOpen = false;
                log.ActionType = "Closed";
                log.LogDetails = $"{updateWindowDto.WindowName} Window is Closed!";
                log.FacilityType = FacilityTypeApp.Window;
                log.FacilityId = updateWindowDto.Id;
            }
            else if(getWindow != null && getWindow.Data.IsOpen == false && getWindow.Data.IsLocked == false)
            {
                updateWindowDto.IsOpen = true;
                log.ActionType = "Open";
                log.LogDetails = $"{updateWindowDto.WindowName} Window is Open!";
                log.FacilityType = FacilityTypeApp.Window;
                log.FacilityId = updateWindowDto.Id;
            }
            updateWindowDto.UnlockedBy = auth.Id;
            var door = await _windowService.UpdateWindow(updateWindowDto, auth.Id);
            if (door.Status == true)    await _logService.CreateLog(log);
            return door;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> LockWindow(GetAuthControlInfoDto getAuthControlInfoDto, UpdateWindowDto updateWindowDto)
    {
        var fail = _authControl.AuthFaliure();
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var getDoor = await _windowService.GetById(updateWindowDto.Id);
            if(getDoor != null && getDoor.Data.IsLocked == true)
            {
                var personCheck = await _authControl.GetPersonDetails(getDoor.Data.LockedBy);
                if(_authControl.CompareRole(personCheck.Role, auth.Role))
                {
                    updateWindowDto.UnlockedBy = auth.Id;
                    updateWindowDto.IsLocked = false;
                    var door = await _windowService.UpdateWindow(updateWindowDto, auth.Id);
                    if (door.Status == true)
                    {
                        var log = _authControl.CreateLog(auth.Id);
                        log.ActionType = "Unlock";
                        log.LogDetails = $"{updateWindowDto.WindowName} Window was unlocked!";
                        log.FacilityType = FacilityTypeApp.Window;
                        log.FacilityId = updateWindowDto.Id;
                        await _logService.CreateLog(log);
                    }
                    return door;
                }
                fail.Message = "Persmission Required";
                return fail;
            }
            else if (getDoor != null && getDoor.Data.IsLocked == false)
            {
                updateWindowDto.UnlockedBy = auth.Id;
                updateWindowDto.IsLocked = true;
                updateWindowDto.IsOpen = false;
                var door = await _windowService.UpdateWindow(updateWindowDto, auth.Id);
                if (door.Status == true)
                {
                    var log = _authControl.CreateLog(auth.Id);
                    log.ActionType = "Lock";
                    log.LogDetails = $"{updateWindowDto.WindowName} Window was locked!";
                    log.FacilityType = FacilityTypeApp.Window;
                    log.FacilityId = updateWindowDto.Id;
                    await _logService.CreateLog(log);
                }
                return door;
            }
            fail.Message = "Unauthorized Action";
            return fail;           
        }
        return _authControl.AuthFaliure();
    }
    public async Task<WindowResponseModel> GetWindowById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var window = await _windowService.GetById(id);
            window.Data = DtoRefactor(window.Data);
            if (window.Data.IsHidden && auth.Role == Role.Owner || auth.Role == Role.Wife)
                return window;
            else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
            {
                return new WindowResponseModel()
                {
                    Data = null,
                    Status = _authControl.AuthFaliure().Status,
                    Message = "Unauthorized Action"
                };
            }
        }
        return new WindowResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<WindowsResponseModel> GetAllWindowsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var window = await _windowService.GetAllWindowsByRoomId(roomId);
            window.Data = window.Data.Select(x => DtoRefactor(x)).ToList();
            return window;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var window = await _windowService.GetAllWindowsByRoomId(roomId);
            window.Data = window.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return window;
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
            window.Data = window.Data.Select(x => DtoRefactor(x)).ToList();
            return window;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var window = await _windowService.GetAllWindowsBySectionId(sectionId);
            window.Data = window.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return window;
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
            window.Data = window.Data.Select(x => DtoRefactor(x)).ToList();
            return window;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var window = await _windowService.GetAllWindows();
            window.Data = window.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return window;
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
                var getWindow = _authControl.GetWindowName(windowId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getWindow} Window was Deleted!";
                log.FacilityType = FacilityTypeApp.Window;
                log.FacilityId = windowId;
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
    public GetWindowDto DtoRefactor(GetWindowDto x)
    {
        x.LastModifiedByName = _authControl.GetUserPersonName(x.LastModifiedBy);
        x.PeronName = _authControl.GetUserPersonName(x.personId);
        x.CreatedByName = _authControl.GetUserPersonName(x.CreatedBy);
        x.RoomName = _authControl.GetRoomName(x.RoomId ?? 0);
        x.SectionName = _authControl.GetSectionName(x.SectionId ?? 0);
        return x;
    }
}