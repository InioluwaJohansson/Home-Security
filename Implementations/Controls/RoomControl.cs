using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Controls.Defaults;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Controls;
public class RoomControl : IRoomControl
{
    IAuthControl _authControl;
    IRoomService _roomService;
    ILogService _logService;
    IObjectDefault _objectDefault;
    public RoomControl(IAuthControl authControl, IRoomService roomService, ILogService logService, IObjectDefault objectDefault)
    {
        _authControl = authControl;
        _roomService = roomService;
        _logService = logService;
        _objectDefault = objectDefault;
    }
    public async Task<BaseResponse> CreateRoom(GetAuthControlInfoDto getAuthControlInfoDto, CreateRoomDto createRoomDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if(auth.Status != false && auth.Role == Role.Owner)
        {
            createRoomDto.PersonId = auth.Id;
            var room = await _roomService.CreateRoom(createRoomDto);
            if (room.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "A Room was newly added. Awaiting Physical Interfacing!";
                await _logService.CreateLog(log);
            }
            return room;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateRoom(GetAuthControlInfoDto getAuthControlInfoDto, UpdateRoomDto updateRoomDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            updateRoomDto.PersonId = auth.Id;
            var room = await _roomService.UpdateRoom(updateRoomDto);
            if (room.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateRoomDto.RoomName} Room was recently updated!";
                await _logService.CreateLog(log);
            }
            return room;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<RoomResponseModel> GetRoomById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var room = await _roomService.GetRoomById(id);
            return room;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new RoomResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new RoomResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<RoomsResponseModel> GetRoomByRoomName(GetAuthControlInfoDto getAuthControlInfoDto, string roomName)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var room = await _roomService.GetRoomByRoomName(roomName);
            return room;
        }
        else if(auth.Status != false && auth.Role == Role.Child || auth.Role ==  Role.Relative || auth.Role == Role.Visitor)
        {
            return new RoomsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new RoomsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<RoomsResponseModel> GetAllRoomsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var room = await _roomService.GetAllRoomsBySectionId(sectionId);
            return room;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new RoomsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new RoomsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<RoomsResponseModel> GetAllRooms(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var room = await _roomService.GetAllRooms();
            return room;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new RoomsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new RoomsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<BaseResponse> DeleteRoom(GetAuthControlInfoDto getAuthControlInfoDto, int roomId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var room = await _roomService.Delete(roomId, auth.Id);
            if (room.Status == true)
            {
                var getRoom = _objectDefault.RoomName(roomId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getRoom} Room was Deleted!";
                await _logService.CreateLog(log);
            }
            return room;
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