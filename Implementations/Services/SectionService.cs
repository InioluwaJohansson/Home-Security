using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.SignalR;
using Home_Security.Hubs;
using Home_Security.RealTimeServices;
using Home_Security.Entities.Identity;

namespace Home_Security.Implementations.Services;
public class SectionService : ISectionService
{
    ISectionRepo _sectionRepo;
    IRealtimeNotificationService _realtime;

    public SectionService(ISectionRepo sectionRepo,IRealtimeNotificationService realtime)
    {
        _sectionRepo = sectionRepo;
        _realtime = realtime;
    }

    public async Task<BaseResponse> CreateSection(int personId, CreateSectionDto createSectionDto)
    {
        if (createSectionDto != null)
        {
            var Section = new Section()
            {
                SectionId = $"SECTION{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                SectionName = createSectionDto.SectionName,
                CreatedOn = DateTime.Now,
                CreatedBy = personId,
                LastModifiedBy = personId,
                LastModifiedOn = DateTime.Now,
                IsHidden = false,
                IsDeleted = false,
            };
            await _sectionRepo.Create(Section);
            var sectionData = await _sectionRepo.Get(x => x.SectionId == Section.SectionId);
            await _realtime.NotifyAll("SectionCreated", GetDetails(await _sectionRepo.GetById(sectionData.Id)));
            return new BaseResponse()
            {
                Status = true,
                Message = "Section Added Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Add Section!"
        };
    }
    public async Task<BaseResponse> UpdateSection(int personId, UpdateSectionDto updateSectionDto)
    {
        var Section = await _sectionRepo.GetById(updateSectionDto.Id);
        if (Section != null)
        {
            Section.SectionName = updateSectionDto.SectionName ?? Section.SectionName;
            Section.LastModifiedBy = personId;
            Section.LastModifiedOn = DateTime.Now;
            if(Section.IsHidden != updateSectionDto.IsHidden){
                foreach(var room in Section.Room) room.IsHidden = updateSectionDto.IsHidden;
                foreach(var appliance in Section.Appliance) appliance.IsHidden = updateSectionDto.IsHidden;
                foreach(var light in Section.Light) light.IsHidden = updateSectionDto.IsHidden;
                foreach(var external in Section.External) external.IsHidden = updateSectionDto.IsHidden;
                foreach(var door in Section.Door) door.IsHidden = updateSectionDto.IsHidden;
                foreach(var window in Section.Window) window.IsHidden = updateSectionDto.IsHidden;
                foreach(var camera in Section.Camera) camera.IsHidden = updateSectionDto.IsHidden;
                Section.IsHidden = updateSectionDto.IsHidden;
            }
            var section = GetDetails(await _sectionRepo.Update(Section));
            if(section.IsHidden){ await _realtime.NotifyAll("SectionUpdated", new List<Role> { Role.Owner, Role.Wife }, section);}
            else await _realtime.NotifyAll("SectionUpdated", section);
            return new BaseResponse()
            {
                Status = true,
                Message = "Section Updated Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update Section!"
        };
    }
    public async Task<BaseResponse> Delete(int id, int personId)
    {
        var Section = await _sectionRepo.Get(x => x.Id == id && x.IsDeleted == false);
        if (Section != null)
        {
            Section.IsDeleted = true;
            Section.DeletedOn = DateTime.Now;
            Section.DeletedBy = personId;
            Section.LastModifiedBy = personId;
            Section.LastModifiedOn = DateTime.Now;
            await _sectionRepo.Update(Section);
            await _realtime.NotifyAll("SectionDeleted", new
            {
                Section.Id,
            });
            return new BaseResponse()
            {
                Status = true,
                Message = "Section Deleted Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Section!"
        };
    }
    public async Task<SectionResponseModel> GetSectionById(int id)
    {
        var Section = await _sectionRepo.GetById(id);
        if (Section != null)
        {
            return new SectionResponseModel()
            {
                Data = GetDetails(Section),
                Status = true,
                Message = "Section Retrieved Successfully!"
            };
        }
        return new SectionResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Section!"
        };
    }
    public async Task<SectionsResponseModel> GetSectionBySectionName(string SectionName)
    {
        var Sections = await _sectionRepo.GetBySectionName(SectionName);
        if (Sections != null)
        {
            return new SectionsResponseModel()
            {
                Data = Sections.Select(x => GetDetails(x)).ToList(),
                Status = true,
                Message = "Sections Retrieved Successfully!"
            };
        }
        return new SectionsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Sections!"
        };
    }
    public async Task<SectionsResponseModel> GetAllSections()
    {
        var Sections = await _sectionRepo.List();
        if (Sections != null)
        {
            return new SectionsResponseModel()
            {
                Data = Sections.Select(x => GetDetails(x)).ToList(),
                Status = true,
                Message = "Sections Retrieved Successfully!"
            };
        }
        return new SectionsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Sections!"
        };
    }
    public GetSectionDto GetDetails(Section Section)
    {
        return new GetSectionDto()
        {
            Id = Section.Id,
            SectionId = Section.SectionId,
            SectionName = Section.SectionName,
            IsHidden = Section.IsHidden,
            LastModifiedBy = Section.LastModifiedBy,
            LastModifiedOn = Section.LastModifiedOn,
            CreatedBy = Section.CreatedBy,
            CreatedOn = Section.CreatedOn,
            Doors = Section.Door == null ? new List<GetDoorDto>() : Section.Door.Select(x => GetDoorDetails(x)).ToList(),
            Appliances = Section.Appliance == null ? new List<GetApplianceDto>() : Section.Appliance.Select(x => GetApplianceDetails(x)).ToList(),
            Lights = Section.Light == null ? new List<GetLightDto>() : Section.Light.Select(x => GetLightDetails(x)).ToList(),
            Windows = Section.Window == null ? new List<GetWindowDto>() : Section.Window.Select(x => GetWindowDetails(x)).ToList(),
            Rooms = Section.Room == null ? new List<GetRoomDto>() : Section.Room.Select(x => GetRoomDetails(x)).ToList(),
            Cameras = Section.Camera == null ? new List<GetCameraDto>() : Section.Camera.Select(x => GetCameraDetails(x)).ToList(),
            Externals = Section.External == null ? new List<GetExternalDto>() : Section.External.Select(x => GetExternalDetails(x)).ToList(),
        };
    }
    public GetRoomDto GetRoomDetails(Room room)
    {
        return new GetRoomDto()
        {
            Id = room.Id,
            RoomId = room.RoomId,
            RoomName = room.RoomName,
            SectionId = room.SectionId,
            personId = room.PersonId,
            Doors = room.Door == null ? new List<GetDoorDto>() : room.Door.Select(x => GetDoorDetails(x)).ToList(),
            Appliances = room.Appliance == null ? new List<GetApplianceDto>() : room.Appliance.Select(x => GetApplianceDetails(x)).ToList(),
            Lights = room.Light == null ? new List<GetLightDto>() : room.Light.Select(x => GetLightDetails(x)).ToList(),
            Windows = room.Window == null ? new List<GetWindowDto>() : room.Window.Select(x => GetWindowDetails(x)).ToList(),
            Cameras = room.Camera == null ? new List<GetCameraDto>() : room.Camera.Select(x => GetCameraDetails(x)).ToList(),
            Externals = room.External == null ? new List<GetExternalDto>() : room.External.Select(x => GetExternalDetails(x)).ToList(),
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
            SectionId = appliance.SectionId,
            RoomId = appliance.RoomId,
            ApplianceId = appliance.ApplianceId,
            LastModifiedBy = appliance.LastModifiedBy,
            DeletedBy = appliance.DeletedBy,
            CreatedBy = appliance.CreatedBy,
            LastModifiedOn = appliance.LastModifiedOn,
            CreatedOn = appliance.CreatedOn,
            IsHidden = appliance.IsHidden,
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
            IsHidden = camera.IsHidden,
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
            IsHidden = External.IsHidden,
        };
    }
    public GetDoorDto GetDoorDetails(Door door)
    {
        return new GetDoorDto()
        {
            Id = door.Id,
            DoorName = door.DoorName,
            DoorType = door.DoorType.ToString(),
            IsLocked = door.IsLocked,
            IsOpen = door.IsOpen,
            OpenedBy = door.OpenedBy,
            LockedBy = door.LockedBy,
            UnlockedBy = door.UnlockedBy,
            IsActive = door.IsActive,
            PowerActive = door.PowerActive,
            SectionId = door.RoomId,
            RoomId = door.SectionId,
            DoorId = door.DoorId,
            LastModifiedBy = door.LastModifiedBy,
            DeletedBy = door.DeletedBy,
            CreatedBy = door.CreatedBy,
            LastModifiedOn = door.LastModifiedOn,
            CreatedOn = door.CreatedOn,
            IsHidden = door.IsHidden,
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
            SectionId = light.SectionId,
            RoomId = light.RoomId,
            LightId = light.LightId,
            LastModifiedBy = light.LastModifiedBy,
            DeletedBy = light.DeletedBy,
            CreatedBy = light.CreatedBy,
            LastModifiedOn = light.LastModifiedOn,
            CreatedOn = light.CreatedOn,
            BrightnessLevel = light.BrightnessLevel,
            IsHidden = light.IsHidden,
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
            SectionId = window.SectionId,
            RoomId = window.RoomId,
            WindowId = window.WindowId,
            LastModifiedBy = window.LastModifiedBy,
            DeletedBy = window.DeletedBy,
            CreatedBy = window.CreatedBy,
            LastModifiedOn = window.LastModifiedOn,
            CreatedOn = window.CreatedOn,
            IsHidden = window.IsHidden,
        };
    }
}