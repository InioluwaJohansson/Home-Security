using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.RealTimeServices;
using Microsoft.AspNetCore.SignalR;

namespace Home_Security.Implementations.Services;
public class RoomService : IRoomService
{
    IRoomRepo _roomRepo;
    IRealtimeNotificationService _realtime;
    public RoomService(IRoomRepo roomRepo, IRealtimeNotificationService realtime)
    {
        _roomRepo = roomRepo;
        _realtime = realtime;
    }
    public async Task<BaseResponse> CreateRoom(CreateRoomDto createRoomDto, int personId)
    {
        if(createRoomDto != null)
        {
            var room = new Room()
            {
                RoomId = $"ROOM{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                PersonId = createRoomDto.PersonId,
                SectionId = createRoomDto.SectionId,
                RoomName = createRoomDto.RoomName,
                Icon = createRoomDto.Icon,
                CreatedOn = DateTime.Now,
                CreatedBy = personId,
                LastModifiedBy = personId,
                LastModifiedOn = DateTime.Now,
                IsDeleted = false,
            };
            await _roomRepo.Create(room);
            var roomData = await _roomRepo.Get(x => x.RoomId == room.RoomId);
            await _realtime.NotifyAll("RoomCreated", GetDetails(await _roomRepo.GetById(roomData.Id)));

            return new BaseResponse()
            {
                Status = true,
                Message = "Room Added Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Add Room!"
        };
    }
    public async Task<BaseResponse> UpdateRoom(UpdateRoomDto updateRoomDto, int personId)
    {
        var room = await _roomRepo.GetById(updateRoomDto.Id);
        if (room != null)
        {
            room.PersonId = updateRoomDto.PersonId;
            room.SectionId = updateRoomDto.SectionId;
            room.RoomName = updateRoomDto.RoomName ?? room.RoomName;
            room.Icon = updateRoomDto.Icon ?? room.Icon;
            if(room.IsHidden != updateRoomDto.IsHidden){
                foreach(var appliance in room.Appliance) appliance.IsHidden = updateRoomDto.IsHidden;
                foreach(var light in room.Light) light.IsHidden = updateRoomDto.IsHidden;
                foreach(var external in room.External) external.IsHidden = updateRoomDto.IsHidden;
                foreach(var door in room.Door) door.IsHidden = updateRoomDto.IsHidden;
                foreach(var window in room.Window) window.IsHidden = updateRoomDto.IsHidden;
                foreach(var camera in room.Camera) camera.IsHidden = updateRoomDto.IsHidden;
                room.IsHidden = updateRoomDto.IsHidden;
            }
            room.LastModifiedBy = personId;
            room.LastModifiedOn = DateTime.Now;

            await _roomRepo.Update(room);

            await _realtime.NotifyAll("RoomUpdated", GetDetails(room));

            return new BaseResponse()
            {
                Status = true,
                Message = "Room Updated Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update Room!"
        };
    }
    public async Task<RoomResponseModel> GetRoomById(int id)
    {
        var room = await _roomRepo.GetById(id);
        if (room != null)
        {
            return new RoomResponseModel()
            {
                Data = GetDetails(room),
                Status = true,
                Message = "Room Retrieved Successfully!"
            };
        }
        return new RoomResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Room!"
        };
    }
    public async Task<RoomsResponseModel> GetRoomByPersonId(int id)
    {
        var rooms = await _roomRepo.GetByPersonId(id);
        if (rooms != null)
        {
            return new RoomsResponseModel()
            {
                Data = rooms.Select(x => GetDetails(x)).ToList(),
                Status = true,
                Message = "Rooms Retrieved Successfully!"
            };
        }
        return new RoomsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Room!"
        };
    }
    public async Task<RoomsResponseModel> GetRoomByRoomName(string roomName)
    {
        var rooms = await _roomRepo.GetByRoomName(roomName);
        if (rooms != null)
        {
            return new RoomsResponseModel()
            {
                Data = rooms.Select(x => GetDetails(x)).ToList(),
                Status = true,
                Message = "Rooms Retrieved Successfully!"
            };
        }
        return new RoomsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Rooms!"
        };
    }
    public async Task<RoomsResponseModel> GetAllRoomsBySectionId(int sectionId)
    {
        var rooms = await _roomRepo.GetBySectionId(sectionId);
        if (rooms != null)
        {
            return new RoomsResponseModel()
            {
                Data = rooms.Select(x => GetDetails(x)).ToList(),
                Status = true,
                Message = "Rooms Retrieved Successfully!"
            };
        }
        return new RoomsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Rooms!"
        };
    }
    public async Task<RoomsResponseModel> GetAllRooms()
    {
        var rooms = await _roomRepo.List();
        if (rooms != null)
        {
            return new RoomsResponseModel()
            {
                Data = rooms.Select(x => GetDetails(x)).ToList(),
                Status = true,
                Message = "Rooms Retrieved Successfully!"
            };
        }
        return new RoomsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Rooms!"
        };
    }
    public async Task<BaseResponse> Delete(int id, int personId)
    {
        var room = await _roomRepo.Get(x => x.Id == id && x.IsDeleted == false);
        if(room != null)
        {
            room.IsDeleted = true;
            room.DeletedOn = DateTime.Now;
            room.DeletedBy = personId;
            room.LastModifiedBy = personId;
            room.LastModifiedOn = DateTime.Now;

            await _roomRepo.Update(room);

            await _realtime.NotifyAll("RoomDeleted", id);

            return new BaseResponse()
            {
                Status = true,
                Message = "Room Deleted Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Room!"
        };
    }
    public GetRoomDto GetDetails(Room room)
    {
        return new GetRoomDto()
        {
            Id = room.Id,
            RoomId = room.RoomId,
            Name = room.RoomName,
            SectionId = room.SectionId,
            personId = room.PersonId,
            Icon = room.Icon,
            IsHidden = room.IsHidden,
            Doors = room.Door == null ? new List<GetDoorDto>() : room.Door.Select(x => GetDoorDetails(x)).ToList(),
            Appliances = room.Appliance == null ? new List<GetApplianceDto>() : room.Appliance.Select(x => GetApplianceDetails(x)).ToList(),
            Lights = room.Light == null ? new List<GetLightDto>() : room.Light.Select(x => GetLightDetails(x)).ToList(),
            Windows = room.Window == null ? new List<GetWindowDto>() : room.Window.Select(x => GetWindowDetails(x)).ToList(),
            Cameras = room.Camera == null ? new List<GetCameraDto>() : room.Camera.Select(x => GetCameraDetails(x)).ToList(),
            Externals = room.External == null ? new List<GetExternalDto>() : room.External.Select(x => GetExternalDetails(x)).ToList(),
            LastModifiedBy = room.LastModifiedBy,
            CreatedBy = room.CreatedBy,
            LastModifiedOn = room.LastModifiedOn,
            CreatedOn = room.CreatedOn
        };
    }
    public GetApplianceDto GetApplianceDetails(Appliance appliance)
    {
        return new GetApplianceDto()
        {
            Id = appliance.Id,
            Name = appliance.ApplianceName,
            IsActive = appliance.IsActive,
            PowerActive = appliance.PowerActive,
            RoomId = appliance.RoomId,
            SectionId = appliance.SectionId,
            ApplianceId = appliance.ApplianceId,
            LastModifiedBy = appliance.LastModifiedBy,
            DeletedBy = appliance.DeletedBy,
            CreatedBy = appliance.CreatedBy,
            LastModifiedOn = appliance.LastModifiedOn,
            CreatedOn = appliance.CreatedOn,
            IsDeleted = appliance.IsDeleted,
        };
    }
    public GetDoorDto GetDoorDetails(Door door)
    {
        return new GetDoorDto()
        {
            Id = door.Id,
            DoorName = door.DoorName,
            DoorType = door.DoorType,
            IsLocked = door.IsLocked,
            IsOpen = door.IsOpen,
            OpenedBy = door.OpenedBy,
            LockedBy = door.LockedBy,
            UnlockedBy = door.UnlockedBy,
            IsActive = door.IsActive,
            PowerActive = door.PowerActive,
            RoomId = door.RoomId,
            SectionId = door.SectionId,
            DoorId = door.DoorId,
            LastModifiedBy = door.LastModifiedBy,
            DeletedBy = door.DeletedBy,
            CreatedBy = door.CreatedBy,
            LastModifiedOn = door.LastModifiedOn,
            CreatedOn = door.CreatedOn,
            IsDeleted = door.IsDeleted,
        };
    }
    public GetCameraDto GetCameraDetails(Camera camera)
    {
        return new GetCameraDto
        {
            Id = camera.Id,
            CameraName = camera.CameraName,
            CameraId = camera.CameraId,

            IsActive = camera.IsActive,
            PowerActive = camera.PowerActive,

            RoomId = camera.RoomId,
            SectionId = camera.SectionId,

            LastModifiedBy = camera.LastModifiedBy,
            DeletedBy = camera.DeletedBy,
            CreatedBy = camera.CreatedBy,

            LastModifiedOn = camera.LastModifiedOn,
            CreatedOn = camera.CreatedOn,

            LiveStreamUrl = camera.LiveStreamUrl,
            Recordings = camera.Recordings.Select(x => new GetRecordingDto
            {
                FilePath = x.FilePath,
                StartTime = x.StartTime,
                EndTime = x.EndTime
            }).ToList()
        };
    }
    public GetExternalDto GetExternalDetails(External External)
    {
        return new GetExternalDto()
        {
            Id = External.Id,
            ExternalName = External.ExternalName,
            IsTriggered = External.IsTriggered,
            IsActive = External.IsActive,
            PowerActive = External.PowerActive,
            RoomId = External.RoomId,
            SectionId = External.SectionId,
            ExternalId = External.ExternalId,
            LastModifiedBy = External.LastModifiedBy,
            DeletedBy = External.DeletedBy,
            CreatedBy = External.CreatedBy,
            LastModifiedOn = External.LastModifiedOn,
            CreatedOn = External.CreatedOn,
            IsDeleted = External.IsDeleted,
        };
    }
    public GetLightDto GetLightDetails(Light light)
    {
        return new GetLightDto()
        {
            Id = light.Id,
            LightName = light.LightName,
            IsActive = light.IsActive,
            PowerActive = light.PowerActive,
            RoomId = light.RoomId,
            SectionId = light.SectionId,
            LightId = light.LightId,
            LastModifiedBy = light.LastModifiedBy,
            DeletedBy = light.DeletedBy,
            CreatedBy = light.CreatedBy,
            LastModifiedOn = light.LastModifiedOn,
            CreatedOn = light.CreatedOn,
            IsDeleted = light.IsDeleted,
            BrightnessLevel = light.BrightnessLevel,
        };
    }
    public GetWindowDto GetWindowDetails(Window window)
    {
        return new GetWindowDto()
        {
            Id = window.Id,
            WindowName = window.WindowName,
            IsLocked = window.IsLocked,
            IsOpen = window.IsOpen,
            OpenedBy = window.OpenedBy,
            LockedBy = window.LockedBy,
            UnlockedBy = window.UnlockedBy,
            IsActive = window.IsActive,
            PowerActive = window.PowerActive,
            RoomId = window.RoomId,
            SectionId = window.SectionId,
            WindowId = window.WindowId,
            LastModifiedBy = window.LastModifiedBy,
            DeletedBy = window.DeletedBy,
            CreatedBy = window.CreatedBy,
            LastModifiedOn = window.LastModifiedOn,
            CreatedOn = window.CreatedOn,
            IsDeleted = window.IsDeleted,
            IsHidden = window.IsHidden,
        };
    }
}