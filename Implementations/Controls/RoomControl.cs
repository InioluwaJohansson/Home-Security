using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
namespace Home_Security.Implementations.Controls;
public class RoomControl : IRoomControl
{
    IAuthControl _authControl;
    IRoomService _roomService;
    ILogService _logService;
    public RoomControl(IAuthControl authControl, IRoomService roomService, ILogService logService)
    {
        _authControl = authControl;
        _roomService = roomService;
        _logService = logService;
    }
    public async Task<BaseResponse> CreateRoom(GetAuthControlInfoDto getAuthControlInfoDto, CreateRoomDto createRoomDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if(auth.Status != false && auth.Role == Role.Owner)
        {
            var room = await _roomService.CreateRoom(createRoomDto, auth.Id);
            if (room.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = $"{createRoomDto.RoomName} Room was newly added. Awaiting Physical Interfacing!";
                log.FacilityType = FacilityTypeApp.Room;
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
            var room = await _roomService.UpdateRoom(updateRoomDto, auth.Id);
            if (room.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateRoomDto.RoomName} Room was recently updated!";
                log.FacilityType = FacilityTypeApp.Room;
                log.FacilityId = updateRoomDto.Id;
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
            room.Data = DtoRefactor(room.Data);
            if (room.Data.IsHidden && auth.Role == Role.Owner || auth.Role == Role.Wife)
                return room;
            else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
            {
                return new RoomResponseModel()
                {
                    Data = null,
                    Status = _authControl.AuthFaliure().Status,
                    Message = "Unauthorized Action"
                };
            }
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
            room.Data = room.Data.Select(x => DtoRefactor(x)).ToList();
            return room;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var room = await _roomService.GetRoomByRoomName(roomName);
            room.Data = room.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return room;
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
            room.Data = room.Data.Select(x => DtoRefactor(x)).ToList();
            return room;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var room = await _roomService.GetAllRoomsBySectionId(sectionId);
            room.Data = room.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return room;
        }
        return new RoomsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<RoomsResponseModel> GetAllRoomsByPersonId(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var room = await _roomService.GetRoomByPersonId(auth.Id);
            room.Data = room.Data.Select(x => DtoRefactor(x)).ToList();
            return room;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var room = await _roomService.GetRoomByPersonId(auth.Id);
            room.Data = room.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return room;
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
            room.Data = room.Data.Select(x => DtoRefactor(x)).ToList();
            return room;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var room = await _roomService.GetAllRooms();
            room.Data = room.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return room;
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
                var getRoom = _authControl.GetRoomName(roomId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getRoom} Room was Deleted!";
                log.FacilityType = FacilityTypeApp.Room;
                log.FacilityId = roomId;
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
    public GetRoomDto DtoRefactor(GetRoomDto x)
    {
        x.LastModifiedByName = _authControl.GetUserPersonName(x.LastModifiedBy);
        x.PeronName = _authControl.GetUserPersonName(x.personId ?? 0);
        x.CreatedByName = _authControl.GetUserPersonName(x.CreatedBy);
        return x;
    }
}