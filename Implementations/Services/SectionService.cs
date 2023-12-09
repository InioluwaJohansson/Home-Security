using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;

namespace Home_Security.Implementations.Services;
public class SectionService : ISectionService
{
    ISectionRepo _sectionRepo;
    IRoomRepo _roomRepo;
    ILightRepo _lightRepo;
    IDoorRepo _doorRepo;
    IWindowRepo _windowRepo;
    IApplianceRepo _applianceRepo;
    public SectionService(ISectionRepo sectionRepo, IRoomRepo roomRepo, ILightRepo lightRepo, IDoorRepo doorRepo, IWindowRepo windowRepo, IApplianceRepo applianceRepo)
    {
        _sectionRepo = sectionRepo;
        _roomRepo = roomRepo;
        _lightRepo = lightRepo;
        _doorRepo = doorRepo;
        _windowRepo = windowRepo;
        _applianceRepo = applianceRepo;
    }
    public async Task<BaseResponse> CreateSection(int personId, CreateSectionDto createSectionDto)
    {
        if (createSectionDto != null)
        {
            var Section = new Section()
            {
                SectionId = $"Section{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                SectionName = createSectionDto.SectionName,
                CreatedOn = DateTime.Now,
                CreatedBy = personId,
                LastModifiedBy = personId,
                LastModifiedOn = DateTime.Now,
                IsDeleted = false,
            };
            await _sectionRepo.Create(Section);
            return new BaseResponse()
            {
                Status = false,
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
        var Section = await _sectionRepo.Get(x => x.Id == updateSectionDto.Id && x.IsDeleted == false);
        if (Section != null)
        {
            Section.SectionName = updateSectionDto.SectionName ?? Section.SectionName;
            Section.LastModifiedBy = personId;
            Section.LastModifiedOn = DateTime.Now;
            await _sectionRepo.Update(Section);
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
    public GetSectionDto GetDetails(Section Section)
    {
        return new GetSectionDto()
        {
            Id = Section.Id,
            SectionId = Section.SectionId,
            SectionName = Section.SectionName,
            Door = Section.Door.Select(x => GetDoorDetails(x)).ToList(),
            Appliance = Section.Appliance.Select(x => GetApplianceDetails(x)).ToList(),
            Light = Section.Light.Select(x => GetLightDetails(x)).ToList(),
            Window = Section.Window.Select(x => GetWindowDetails(x)).ToList(),
            Room = Section.Room.Select(x => GetRoomDetails(x)).ToList(),
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
            PersonId = room.PersonId,
            Doors = room.Door.Select(x => GetDoorDetails(x)).ToList(),
            Appliances = room.Appliance.Select(x => GetApplianceDetails(x)).ToList(),
            Lights = room.Light.Select(x => GetLightDetails(x)).ToList(),
            Windows = room.Window.Select(x => GetWindowDetails(x)).ToList()
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
            SectionId = door.RoomId,
            RoomId = door.SectionId,
            DoorId = door.DoorId,
            LastModifiedBy = door.LastModifiedBy,
            DeletedBy = door.DeletedBy,
            CreatedBy = door.CreatedBy,
            LastModifiedOn = door.LastModifiedOn,
            CreatedOn = door.CreatedOn,
            IsDeleted = door.IsDeleted,
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
            SectionId = window.SectionId,
            RoomId = window.RoomId,
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