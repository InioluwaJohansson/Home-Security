using BCrypt.Net;
using Home_Security.Entities.Identity;
using Home_Security.Entities;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Repositories;
using Home_Security.Models.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Home_Security.Models.Enums;

namespace Home_Security.Implementations.Controls;
public class AuthControl : IAuthControl
{
    public readonly IMemoryCache _cache;
    public readonly IMemoryCache _roomCache;
    public readonly IMemoryCache _sectionCache;
    public readonly IMemoryCache _applianceCache;
    public readonly IMemoryCache _lightCache;
    public readonly IMemoryCache _cameraCache;
    public readonly IMemoryCache _windowCache;
    public readonly IMemoryCache _doorCache;
    public readonly IMemoryCache _actionCache;
    public readonly IMemoryCache _externalCache;
    public readonly IMemoryCache _contactCache;
    public readonly IMemoryCache _contactCategoryCache;
    IPersonRepo _personRepo;
    IUserRepo _userRepo;
    IRoomRepo _roomRepo;
    ISectionRepo _sectionRepo;
    IApplianceRepo _applianceRepo;
    ILightRepo _lightRepo;
    ICameraRepo _cameraRepo;
    IWindowRepo _windowRepo;
    IDoorRepo _doorRepo;
    IActionRepo _actionRepo;
    IExternalRepo _externalRepo;
    IContactRepo _contactRepo;
    IContactCategoryRepo _contactCategoryRepo;
    public AuthControl(IUserRepo userRepo, IMemoryCache cache, IMemoryCache roomCache, IMemoryCache sectionCache, IMemoryCache applianceCache, IMemoryCache lightCache, IMemoryCache cameraCache, IMemoryCache windowCache, IMemoryCache doorCache, IMemoryCache actionCache, IMemoryCache externalCache, IMemoryCache contactCache, IMemoryCache contactCategoryCache, IPersonRepo personRepo, IRoomRepo roomRepo, ISectionRepo sectionRepo,IApplianceRepo applianceRepo,ILightRepo lightRepo, ICameraRepo cameraRepo,IWindowRepo windowRepo, IDoorRepo doorRepo,IActionRepo actionRepo, IExternalRepo externalRepo, IContactRepo contactRepo, IContactCategoryRepo contactCategoryRepo)
    {
        _cache = cache;
        _roomCache = roomCache;
        _sectionCache = sectionCache;
        _applianceCache = applianceCache;
        _lightCache = lightCache;
        _cameraCache = cameraCache;
        _windowCache = windowCache;
        _doorCache = doorCache;
        _actionCache = actionCache;
        _externalCache = externalCache;
        _contactCache = contactCache;
        _contactCategoryCache = contactCategoryCache;
        _userRepo = userRepo;
        _personRepo = personRepo;
        _roomRepo = roomRepo;
        _sectionRepo = sectionRepo;
        _applianceRepo = applianceRepo;
        _lightRepo = lightRepo;
        _cameraRepo = cameraRepo;
        _windowRepo = windowRepo;
        _doorRepo = doorRepo;
        _externalRepo = externalRepo;
        _actionRepo = actionRepo;
        _contactRepo = contactRepo;
        _contactCategoryRepo = contactCategoryRepo;
    }
    public async Task<GetAuthControlDto> GetAuthDetails(int personId, string authorizationCode)
    {
        var person = await _personRepo.GetById(personId);
        if (person != null && BCrypt.Net.BCrypt.EnhancedVerify(authorizationCode, person.User.AuthorizationCode) && person.Disabled == false)
        {
            return new GetAuthControlDto
            {
                Id = person.Id,
                Role = person.User.UserRole.Role,
                Status = true
            };
        }
        return new GetAuthControlDto
        {
            Status = false
        };
    }
    public async Task<GetAuthControlDto> GetPersonDetails(int personId)
    {
        var person = await _personRepo.GetById(personId);
        if (person != null)
        {
            return new GetAuthControlDto
            {
                Id = person.Id,
                Role = person.User.UserRole.Role,
                IsDisabled = person.Disabled,
                Status = true
            };
        }
        return new GetAuthControlDto
        {
            Status = false
        };
    }
    public async Task GetAllPersons()
    {
        var persons = await _personRepo.GetByExpression(x => x.IsDeleted == false);
        if (persons != null && persons.Any())
            foreach (var person in persons) _cache.Set(person.Id, person);
    }
    public string GetUserPersonName(int id)
    {
        if (_cache.TryGetValue(id, out Person person))
        {
            return  $"{person.PersonDetails.FirstName} {person.PersonDetails.LastName}"; 
        }
        return null;
    }
    public async Task GetAllRooms()
    {
        var rooms = await _roomRepo.GetByExpression(x => x.IsDeleted == false);
        if (rooms != null && rooms.Any())
            foreach (var room in rooms) _roomCache.Set(room.Id, room);
    }
    public string GetRoomName(int id)
    {
        if (_roomCache.TryGetValue(id, out Room room))
            return room.RoomName; 
        return null;
    }
    public async Task GetAllSections()
    {
        var sections = await _sectionRepo.GetByExpression(x => x.IsDeleted == false);
        if (sections != null && sections.Any())
            foreach (var section in sections) _sectionCache.Set(section.Id, section);
    }
    public string GetSectionName(int id)
    {
        if (_sectionCache.TryGetValue(id, out Section section))
            return section.SectionName;
        return null;
    }
    public async Task GetAllAppliances()
    {
        var appliances = await _applianceRepo.GetByExpression(x => x.IsDeleted == false);
        if (appliances != null && appliances.Any())
            foreach (var appliance in appliances) _applianceCache.Set(appliance.Id, appliance);
    }
    public string GetApplianceName(int id)
    {
        if (_applianceCache.TryGetValue(id, out Appliance appliance))
            return appliance.ApplianceName; 
        return null;
    }
    public async Task GetAllLights()
    {
        var lights = await _lightRepo.GetByExpression(x => x.IsDeleted == false);
        if (lights != null && lights.Any())
            foreach (var light in lights) _lightCache.Set(light.Id, light);
    }
    public string GetLightName(int id)
    {
        if (_lightCache.TryGetValue(id, out Light light))
            return light.LightName;
        return null;
    }
    public async Task GetAllCameras()
    {
        var cameras = await _cameraRepo.GetByExpression(x => x.IsDeleted == false);
        if (cameras != null && cameras.Any())
            foreach (var camera in cameras) _cameraCache.Set(camera.Id, camera);
    }
    public string GetCameraName(int id)
    {
        if (_cameraCache.TryGetValue(id, out Camera camera))
            return camera.CameraName; 
        return null;
    }
    public async Task GetAllWindows()
    {
        var windows = await _windowRepo.GetByExpression(x => x.IsDeleted == false);
        if (windows != null && windows.Any())
            foreach (var window in windows) _windowCache.Set(window.Id, window);
    }
    public string GetWindowName(int id)
    {
        if (_windowCache.TryGetValue(id, out Window window))
            return window.WindowName; 
        return null;
    }
    public async Task GetAllDoors()
    {
        var doors = await _doorRepo.GetByExpression(x => x.IsDeleted == false);
        if (doors != null && doors.Any())
            foreach (var door in doors) _doorCache.Set(door.Id, door);
    }
    public string GetDoorName(int id)
    {
        if (_doorCache.TryGetValue(id, out Door door))
            return door.DoorName; 
        return null;
    }
    public async Task GetAllActions()
    {
        var actions = await _actionRepo.GetByExpression(x => x.IsDeleted == false);
        if (actions != null && actions.Any())
            foreach (var action in actions) _actionCache.Set(action.Id, action);
    }
    public string GetActionName(int id)
    {
        if (_doorCache.TryGetValue(id, out Entities.Action action))
            return action.ActionName;
        return null;
    }
    public async Task GetAllExternals()
    {
        var externals = await _externalRepo.GetByExpression(x => x.IsDeleted == false);
        if (externals != null && externals.Any())
            foreach (var external in externals) _actionCache.Set(external.Id, external);
    }
    public string GetExternalName(int id)
    {
        if (_externalCache.TryGetValue(id, out External external))
            return external.ExternalName;
        return null;
    }
    public async Task GetAllContacts()
    {
        var contacts = await _contactRepo.GetByExpression(x => x.IsDeleted == false);
        if (contacts != null && contacts.Any())
            foreach (var contact in contacts) _contactCache.Set(contact.Id, contact);
    }
    public string GetContactName(int id)
    {
        if (_contactCache.TryGetValue(id, out Contact contact))
            return $"{contact.FirstName} {contact.LastName}";
        return null;
    }
    public async Task GetAllContactCategories()
    {
        var contactCategories = await _contactCategoryRepo.GetByExpression(x => x.IsDeleted == false);
        if (contactCategories != null && contactCategories.Any())
            foreach (var category in contactCategories) _contactCategoryCache.Set(category.Id, category);
    }
    public string GetContactCategoryName(int id)
    {
        if (_contactCategoryCache.TryGetValue(id, out ContactCategory contactCategory))
            return contactCategory.Name;
        return null;
    }
    public async Task RunAllFacilities()
    {
        _actionCache.Dispose();
        await GetAllActions();
        _applianceCache.Dispose();
        await GetAllAppliances();
        _cameraCache.Dispose();
        await GetAllCameras();
        _lightCache.Dispose();
        await GetAllLights();
        _cache.Dispose();
        await GetAllPersons();
        _roomCache.Dispose();
        await GetAllRooms();
        _sectionCache.Dispose();
        await GetAllSections();
        _doorCache.Dispose();
        await GetAllDoors();
        _externalCache.Dispose();
        await GetAllExternals();
        _windowCache.Dispose();
        await GetAllWindows();
        _contactCategoryCache.Dispose();
        await GetAllContactCategories();
        _contactCache.Dispose();
        await GetAllContacts();
    }
    public bool CompareRole(Role based, Role quote)
    {
        //based is role of last locked user
        //quote is role of user trying to unlock it
        if (quote == Role.Owner || based == Role.Owner) return true;

        else if ((based == Role.Child || based == Role.Relative || based == Role.Visitor) && (quote == Role.Owner || quote == Role.Wife)) return true;

        else if ((based == Role.Relative || based == Role.Visitor) && (quote == Role.Owner || quote == Role.Wife || quote == Role.Child)) return true;

        else if (based == Role.Visitor && (quote == Role.Owner || quote == Role.Wife || quote == Role.Child || based == Role.Relative)) return true;

        return false;
    }
    public BaseResponse AuthFaliure()
    {
        return new BaseResponse()
        {
            Message = "Authentication Faliure!",
            Status = false
        };
    }
    public CreateLogDto CreateLog(int personId)
    {
        return new CreateLogDto()
        {
            PersonId = personId,
            LogDetails = "",
            ActionType = ""
        };
    }
    public async Task<AppNamesDetailList> GetAppNamesDetails()
    {
        return new AppNamesDetailList
        {
            ApplianceIdNames = (await _applianceRepo.GetByExpression(x => x.IsDeleted == false))
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.ApplianceName,
                IsHidden = c.IsHidden,
            }).ToList(),
            CameraIdNames = (await _cameraRepo.GetByExpression(x => x.IsDeleted == false))
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.CameraName,
                IsHidden = c.IsHidden,
            }).ToList(),
            LightIdNames = (await _lightRepo.GetByExpression(x => x.IsDeleted == false))
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.LightName,
                IsHidden = c.IsHidden,
            }).ToList(),
            WindowIdNames = (await _windowRepo.GetByExpression(x => x.IsDeleted == false))
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.WindowName,
                IsHidden = c.IsHidden,
            }).ToList(),
            DoorIdNames = (await _doorRepo.GetByExpression(x => x.IsDeleted == false))
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.DoorName,
                IsHidden = c.IsHidden,
            }).ToList(),
            ExternalIdNames = (await _externalRepo.GetByExpression(x => x.IsDeleted == false))
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.ExternalName,
                IsHidden = c.IsHidden,
            }).ToList(),
            PersonIdNames = (await _userRepo.(x => x.IsDeleted == false))
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c,
                IsHidden = c.,
            }).ToList(),
            ContactCategoryIdNames = (await _contactCategoryRepo.GetByExpression(x => x.IsDeleted == false))
            .Select(c => new GenericIdNames
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList(),
            ApplianceType = (new GenericIdNames(A)).ToList(),
        };
    }
}
