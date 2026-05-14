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
    ILightRepo _lightRepo;
    IDoorRepo _doorRepo;
    IWindowRepo _windowRepo;
    IApplianceRepo _applianceRepo;
    IRealtimeNotificationService _realtime;

    public RoomService(IRoomRepo roomRepo, ILightRepo lightRepo, IDoorRepo doorRepo, IWindowRepo windowRepo, IApplianceRepo applianceRepo, IRealtimeNotificationService realtime)
    {
        _roomRepo = roomRepo;
        _lightRepo = lightRepo;
        _doorRepo = doorRepo;
        _windowRepo = windowRepo;
        _applianceRepo = applianceRepo;
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
                CreatedOn = DateTime.Now,
                CreatedBy = personId,
                LastModifiedBy = personId,
                LastModifiedOn = DateTime.Now,
                IsDeleted = false,
            };
            await _roomRepo.Create(room);

            await _realtime.NotifyAll("RoomCreated", room.Id);

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
        var room = await _roomRepo.Get(x => x.Id == updateRoomDto.Id && x.IsDeleted == false);
        if (room != null)
        {
            room.PersonId = updateRoomDto.PersonId;
            room.SectionId = updateRoomDto.SectionId;
            room.RoomName = updateRoomDto.RoomName ?? room.RoomName;
            room.LastModifiedBy = personId;
            room.LastModifiedOn = DateTime.Now;

            await _roomRepo.Update(room);

            await _realtime.NotifyAll("RoomUpdated", room.Id);

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
            RoomName = room.RoomName,
            SectionId = room.SectionId,
            PersonId = room.PersonId,
            Doors = room.Door.Select(x => GetDoorDetails(x)).ToList(),
            Appliances = room.Appliance.Select(x => GetApplianceDetails(x)).ToList(),
            Lights = room.Light.Select(x => GetLightDetails(x)).ToList(),
            Windows = room.Window.Select(x => GetWindowDetails(x)).ToList(),
            Cameras = room.Camera.Select(x => GetCameraDetails(x)).ToList(),
            Externals = room.External.Select(x => GetExternalDetails(x)).ToList(),
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
            ApplianceName = appliance.ApplianceName,
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
        return new GetCameraDto()
        {
            Id = camera.Id,
            CameraName = camera.CameraName,
            IsActive = camera.IsActive,
            PowerActive = camera.PowerActive,
            RoomId = camera.RoomId,
            SectionId = camera.SectionId,
            CameraId = camera.CameraId,
            LastModifiedBy = camera.LastModifiedBy,
            DeletedBy = camera.DeletedBy,
            CreatedBy = camera.CreatedBy,
            LastModifiedOn = camera.LastModifiedOn,
            CreatedOn = camera.CreatedOn,
            IsDeleted = camera.IsDeleted,
        };
    }
    public GetExternalDto GetExternalDetails(External External)
    {
        return new GetExternalDto()
        {
            Id = External.Id,
            ExternalsName = External.ExternalName,
            IsTriggered = External.IsTriggered,
            IsActive = External.IsActive,
            PowerActive = External.PowerActive,
            RoomId = External.RoomId,
            SectionId = External.SectionId,
            ExternalsId = External.ExternalId,
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
        };
    }
}