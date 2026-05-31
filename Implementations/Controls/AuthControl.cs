using BCrypt.Net;
using Home_Security.Entities.Identity;
using Home_Security.Entities;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Repositories;
using Home_Security.Models.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Home_Security.Models.Enums;
using System.Collections.Generic;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Controls;
public class AuthControl : IAuthControl
{
    public readonly List<Person> _cache =  new ();
    public readonly List<Room> _roomCache =  new ();
    public readonly List<Section> _sectionCache =  new ();
    public readonly List<Appliance> _applianceCache =  new ();
    public readonly List<Light> _lightCache =  new ();
    public readonly List<Camera> _cameraCache =  new ();
    public readonly List<Window> _windowCache =  new ();
    public readonly List<Door> _doorCache =  new ();
    public readonly List<Entities.Action> _actionCache =  new ();
    public readonly List<External> _externalCache =  new ();
    public readonly List<Contact> _contactCache =  new ();
    public readonly List<ContactCategory> _contactCategoryCache =  new ();
    public readonly List<Hardware> _hardwareCache =  new ();
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
    IHardwareRepo _hardwareRepo;
    IContactRepo _contactRepo;
    IContactCategoryRepo _contactCategoryRepo;
    IRealtimeNotificationService _realtimeNotificationService;
    public AuthControl(IUserRepo userRepo, IPersonRepo personRepo, IRoomRepo roomRepo, ISectionRepo sectionRepo,IApplianceRepo applianceRepo,ILightRepo lightRepo, ICameraRepo cameraRepo,IWindowRepo windowRepo, IDoorRepo doorRepo,IActionRepo actionRepo, IExternalRepo externalRepo, IHardwareRepo hardwareRepo, IContactRepo contactRepo, IContactCategoryRepo contactCategoryRepo, IRealtimeNotificationService realtimeNotificationService)
    {
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
        _hardwareRepo = hardwareRepo;
        _actionRepo = actionRepo;
        _contactRepo = contactRepo;
        _contactCategoryRepo = contactCategoryRepo;
        _realtimeNotificationService = realtimeNotificationService;
    }
    public async Task<GetAuthControlDto> GetAuthDetails(int personId, string authorizationCode)
    {
        var person = _cache.FirstOrDefault(x => x.Id == personId);
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
        var persons = await _personRepo.List();
        if (persons != null && persons.Any())
            foreach (var person in persons) _cache.Add(person);
    }
    public string GetUserPersonName(int id)
    {
        var person = _cache.FirstOrDefault(x => x.Id == id);
        if (person != null)
        {
            return  $"{person.PersonDetails.FirstName} {person.PersonDetails.LastName}"; 
        }
        return null;
    }
    public async Task GetAllRooms()
    {
        var rooms = await _roomRepo.GetByExpression(x => x.IsDeleted == false);
        if (rooms != null && rooms.Any())
            foreach (var room in rooms) _roomCache.Add(room);
    }
    public string GetRoomName(int id)
    {
        var room = _roomCache.FirstOrDefault(x => x.Id == id);
        if (room != null)
            return room.RoomName; 
        return null;
    }
    public async Task GetAllSections()
    {
        var sections = await _sectionRepo.GetByExpression(x => x.IsDeleted == false);
        if (sections != null && sections.Any())
            foreach (var section in sections) _sectionCache.Add(section);
    }
    public string GetSectionName(int id)
    {
        var section = _sectionCache.FirstOrDefault(x => x.Id == id);
        if (section != null)
            return section.SectionName;
        return null;
    }
    public async Task GetAllAppliances()
    {
        var appliances = await _applianceRepo.GetByExpression(x => x.IsDeleted == false);
        if (appliances != null && appliances.Any())
            foreach (var appliance in appliances) _applianceCache.Add(appliance);
    }
    public string GetApplianceName(int id)
    {
        var appliance = _applianceCache.FirstOrDefault(x => x.Id == id);
        if (appliance != null)
            return appliance.ApplianceName; 
        return null;
    }
    public async Task GetAllLights()
    {
        var lights = await _lightRepo.GetByExpression(x => x.IsDeleted == false);
        if (lights != null && lights.Any())
            foreach (var light in lights) _lightCache.Add(light);
    }
    public string GetLightName(int id)
    {
        var light = _lightCache.FirstOrDefault(x => x.Id == id);
        if (light != null)
            return light.LightName;
        return null;
    }
    public async Task GetAllCameras()
    {
        var cameras = await _cameraRepo.GetByExpression(x => x.IsDeleted == false);
        if (cameras != null && cameras.Any())
            foreach (var camera in cameras) _cameraCache.Add(camera);
    }
    public string GetCameraName(int id)
    {
        var camera = _cameraCache.FirstOrDefault(x => x.Id == id);
        if (camera != null)
            return camera.CameraName; 
        return null;
    }
    public async Task GetAllWindows()
    {
        var windows = await _windowRepo.GetByExpression(x => x.IsDeleted == false);
        if (windows != null && windows.Any())
            foreach (var window in windows) _windowCache.Add(window);
    }
    public string GetWindowName(int id)
    {
        var window = _windowCache.FirstOrDefault(x => x.Id == id);
        if (window != null)
            return window.WindowName; 
        return null;
    }
    public async Task GetAllDoors()
    {
        var doors = await _doorRepo.GetByExpression(x => x.IsDeleted == false);
        if (doors != null && doors.Any())
            foreach (var door in doors) _doorCache.Add(door);
    }
    public string GetDoorName(int id)
    {
        var door = _doorCache.FirstOrDefault(x => x.Id == id);
        if (door != null)
            return door.DoorName; 
        return null;
    }
    public async Task GetAllActions()
    {
        var actions = await _actionRepo.GetByExpression(x => x.IsDeleted == false);
        if (actions != null && actions.Any())
            foreach (var action in actions) _actionCache.Add(action);
    }
    public string GetActionName(int id)
    {
        var action = _actionCache.FirstOrDefault(x => x.Id == id);
        if (action != null)
            return action.ActionName;
        return null;
    }
    public async Task GetAllExternals()
    {
        var externals = await _externalRepo.GetByExpression(x => x.IsDeleted == false);
        if (externals != null && externals.Any())
            foreach (var external in externals) _externalCache.Add(external);
    }
    public string GetExternalName(int id)
    {
        var external = _externalCache.FirstOrDefault(x => x.Id == id);
        if (external != null)
            return external.ExternalName;
        return null;
    }
    public async Task GetAllHardwares()
    {
        var hardwares = await _hardwareRepo.GetByExpression(x => x.IsDeleted == false);
        if (hardwares != null && hardwares.Any())
            foreach (var hardware in hardwares) _hardwareCache.Add(hardware);
    }
    public string GetHardwareName(int id)
    {
        var hardware = _hardwareCache.FirstOrDefault(x => x.Id == id);
        if (hardware != null)
            return hardware.HardwareName;
        return null;
    }
    public async Task GetAllContacts()
    {
        var contacts = await _contactRepo.GetByExpression(x => x.IsDeleted == false);
        if (contacts != null && contacts.Any())
            foreach (var contact in contacts) _contactCache.Add(contact);
    }
    public string GetContactName(int id)
    {
        var contact = _contactCache.FirstOrDefault(x => x.Id == id);
        if (contact != null)
            return $"{contact.FirstName} {contact.LastName}";
        return null;
    }
    public async Task GetAllContactCategories()
    {
        var contactCategories = await _contactCategoryRepo.GetByExpression(x => x.IsDeleted == false);
        if (contactCategories != null && contactCategories.Any())
            foreach (var category in contactCategories) _contactCategoryCache.Add(category);
    }
    public string GetContactCategoryName(int id)
    {
        var contactCategory = _contactCategoryCache.FirstOrDefault(x => x.Id == id);
        if (contactCategory != null)
            return contactCategory.Name;
        return null;
    }
    public async Task RunAllFacilities()
    {
        _actionCache.Clear();
        await GetAllActions();
        _applianceCache.Clear();
        await GetAllAppliances();
        _cameraCache.Clear();
        await GetAllCameras();
        _lightCache.Clear();
        await GetAllLights();
        _cache.Clear();
        await GetAllPersons();
        _roomCache.Clear();
        await GetAllRooms();
        _sectionCache.Clear();
        await GetAllSections();
        _doorCache.Clear();
        await GetAllDoors();
        _externalCache.Clear();
        await GetAllExternals();
        _hardwareCache.Clear();
        await GetAllHardwares();
        _windowCache.Clear();
        await GetAllWindows();
        _contactCategoryCache.Clear();
        await GetAllContactCategories();
        _contactCache.Clear();
        await GetAllContacts();
        await _realtimeNotificationService.NotifyAll("GetAppNamesDetails", GetAppNamesDetails());
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
            ApplianceIdNames = _applianceCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.ApplianceName,
                IsHidden = c.IsHidden,
            }).ToList(),
            CameraIdNames = _cameraCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.CameraName,
                IsHidden = c.IsHidden,
            }).ToList(),
            LightIdNames = _lightCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.LightName,
                IsHidden = c.IsHidden,
            }).ToList(),
            WindowIdNames = _windowCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.WindowName,
                IsHidden = c.IsHidden,
            }).ToList(),
            DoorIdNames = _doorCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.DoorName,
                IsHidden = c.IsHidden,
            }).ToList(),
            ExternalIdNames = _externalCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.ExternalName,
                IsHidden = c.IsHidden,
            }).ToList(),
            PersonIdNames = _cache
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.PersonDetails.FirstName + " " + c.PersonDetails.LastName,
                IsHidden = c.Disabled,
                ImageUrl = ConvertImageToBase64(c.PersonDetails.ImageUrl) ?? "",
            }).ToList(),
            ContactCategoryIdNames = _contactCategoryCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericIdNames
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList(),
            ActionIdNames = _actionCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericIdNames
            {
                Id = c.Id,
                Name = c.ActionName,
            }).ToList(),
            HardwareIdNames = _hardwareCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericIdNames
            {
                Id = c.Id,
                Name = c.HardwareName,
            }).ToList(),
            ApplianceType = EnumToList<ApplianceType>(),
            Gender = EnumToList<Gender>(),
            DoorType = EnumToList<DoorType>(),
            FacilityType = EnumToList<FacilityType>(),
            Role = EnumToList<Role>()
        };
    }
    public static string? ConvertImageToBase64(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        if (!File.Exists(path))
            return null;

        var bytes = File.ReadAllBytes(path);

        return Convert.ToBase64String(bytes);
    }
    public static List<GenericIdNames> EnumToList<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
        .Cast<T>()
        .Select(e => new GenericIdNames
        {
            Id = Convert.ToInt32(e),
            Name = e.ToString()
        })
        .ToList();
    }
}
