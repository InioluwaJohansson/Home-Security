using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.Interfaces.Controls.Defaults;

namespace Home_Security.Implementations.Controls;
public class DoorControl : IDoorControl
{
    IAuthControl _authControl;
    IDoorService _doorService;
    ILogService _logService;
    IObjectDefault _objectDefault;
    public DoorControl(IAuthControl authControl, IDoorService doorService, ILogService logService, IObjectDefault objectDefault)
    {
        _authControl = authControl;
        _doorService = doorService;
        _logService = logService;
        _objectDefault = objectDefault;
    }
    public async Task<BaseResponse> CreateDoor(GetAuthControlInfoDto getAuthControlInfoDto, CreateDoorDto createDoorDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            createDoorDto.personId = auth.Id;
            var door = await _doorService.CreateDoor(createDoorDto);
            if (door.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "A Door was newly added. Awaiting Physical Interfacing!";
                await _logService.CreateLog(log);
            }
            return door;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateDoor(GetAuthControlInfoDto getAuthControlInfoDto, UpdateDoorDto updateDoorDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            updateDoorDto.personId = auth.Id;
            var door = await _doorService.UpdateDoor(updateDoorDto);
            if (door.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateDoorDto.DoorName} Door was recently updated!";
                await _logService.CreateLog(log);
            }
            return door;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UnlockDoor(GetAuthControlInfoDto getAuthControlInfoDto, UpdateDoorDto updateDoorDto)
    {
        var fail = _authControl.AuthFaliure();
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var getDoor = await _doorService.GetById(updateDoorDto.Id);
            if(getDoor != null && getDoor.Data.IsLocked == true)
            {
                var personCheck = await _authControl.GetPersonDetails(getDoor.Data.LockedBy);
                if(_authControl.CompareRole(personCheck.Role, auth.Role))
                {
                    updateDoorDto.personId = auth.Id;
                    updateDoorDto.UnlockedBy = auth.Id;
                    updateDoorDto.IsLocked = false;
                    var door = await _doorService.UpdateDoor(updateDoorDto);
                    if (door.Status == true)
                    {
                        var log = _authControl.CreateLog(auth.Id);
                        log.ActionType = "Unlock";
                        log.LogDetails = $"{updateDoorDto.DoorName} Door was unlocked!";
                        await _logService.CreateLog(log);
                    }
                    return door;
                }
                fail.Message = "Persmission Required";
                return fail;
            }
            fail.Message = "Unauthorized Action";
            return fail;           
        }
        else if(getAuthControlInfoDto.PersonId != 0)
        {
            var person = await _objectDefault.PersonName(getAuthControlInfoDto.PersonId);
            if (person != null)
            {
                var getDoor = await _doorService.GetById(updateDoorDto.Id);
                if (getDoor != null && getDoor.Data.IsLocked == true)
                {
                    var personCheck = await _authControl.GetPersonDetails(getDoor.Data.LockedBy);
                    if (_authControl.CompareRole(personCheck.Role, auth.Role))
                    {
                        updateDoorDto.personId = auth.Id;
                        updateDoorDto.UnlockedBy = auth.Id;
                        updateDoorDto.IsLocked = false;
                        var door = await _doorService.UpdateDoor(updateDoorDto);
                        if (door.Status == true)
                        {
                            var log = _authControl.CreateLog(auth.Id);
                            log.ActionType = "Unlock";
                            log.LogDetails = $"{updateDoorDto.DoorName} Door was unlocked!";
                            await _logService.CreateLog(log);
                        }
                        return door;
                    }
                    fail.Message = "Persmission Required!";
                    return fail;
                }
                fail.Message = "Unauthorized Action!";
                return fail;
            }
            fail.Message = "Failed Action!";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> LockDoor(GetAuthControlInfoDto getAuthControlInfoDto, UpdateDoorDto updateDoorDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            updateDoorDto.personId = auth.Id;
            updateDoorDto.IsOpen = false;
            updateDoorDto.LockedBy = auth.Id;
            updateDoorDto.IsLocked = true;
            var door = await _doorService.UpdateDoor(updateDoorDto);
            if (door.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Lock";
                log.LogDetails = $"{updateDoorDto.DoorName} Door is Locked!";
                await _logService.CreateLog(log);
            }
            return door;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<DoorResponseModel> GetDoorById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var door = await _doorService.GetById(id);
            return door;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new DoorResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new DoorResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<DoorsResponseModel> GetAllDoorsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int doorId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var door = await _doorService.GetAllDoorsByRoomId(doorId);
            return door;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new DoorsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new DoorsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<DoorsResponseModel> GetAllDoorsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var door = await _doorService.GetAllDoorsBySectionId(sectionId);
            return door;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new DoorsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new DoorsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<DoorsResponseModel> GetAllDoorsByDoorType(GetAuthControlInfoDto getAuthControlInfoDto, DoorType doorType)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var door = await _doorService.GetAllDoorsByDoorType(doorType);
            return door;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new DoorsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new DoorsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<DoorsResponseModel> GetAllDoors(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var door = await _doorService.GetAllDoors();
            return door;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new DoorsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new DoorsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<BaseResponse> DeleteDoor(GetAuthControlInfoDto getAuthControlInfoDto, int doorId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var door = await _doorService.Delete(doorId, auth.Id);
            if (door.Status == true)
            {
                var getDoor = await _objectDefault.DoorName(doorId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getDoor} Door was Deleted!";
                await _logService.CreateLog(log);
            }
            return door;
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