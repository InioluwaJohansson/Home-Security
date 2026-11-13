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
    IAuthCache _authCache;
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
    IUtilityRepo _utilityRepo;
    IExternalRepo _externalRepo;
    IHardwareRepo _hardwareRepo;
    IContactRepo _contactRepo;
    IContactCategoryRepo _contactCategoryRepo;
    IRealtimeNotificationService _realtimeNotificationService;
    public AuthControl(IAuthCache authCache, IUserRepo userRepo, IPersonRepo personRepo, IRoomRepo roomRepo, ISectionRepo sectionRepo,IApplianceRepo applianceRepo,ILightRepo lightRepo, ICameraRepo cameraRepo,IWindowRepo windowRepo, IDoorRepo doorRepo,IActionRepo actionRepo, IExternalRepo externalRepo, IUtilityRepo utilityRepo, IHardwareRepo hardwareRepo, IContactRepo contactRepo, IContactCategoryRepo contactCategoryRepo, IRealtimeNotificationService realtimeNotificationService)
    {
        _authCache = authCache;
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
        _utilityRepo = utilityRepo;
        _contactRepo = contactRepo;
        _contactCategoryRepo = contactCategoryRepo;
        _realtimeNotificationService = realtimeNotificationService;
    }
    public async Task<GetAuthControlDto> GetAuthDetails(int personId, string authorizationCode)
    {
        if (_authCache._cache == null)
        {
            throw new Exception("_cache is null");
        }
        var person = _authCache._cache.FirstOrDefault(x => x != null && x.Id == personId);
        if (person != null && authorizationCode.Equals(person.User.AuthorizationCode) && person.Disabled == false)
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
    public Person GetPersonById(int personId)
    {
        var person = _authCache._cache.FirstOrDefault(x => x.Id == personId);
        if (person != null) return person;
        return null;
    }
    public async Task GetAllPersons()
    {
        var persons = await _personRepo.List();
        if (persons != null && persons.Any())
            foreach (var person in persons) _authCache._cache.Add(person);
    }
    public string GetUserPersonName(int id)
    {
        var person = _authCache._cache.FirstOrDefault(x => x.Id == id);
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
            foreach (var room in rooms) _authCache._roomCache.Add(room);
    }
    public string GetRoomName(int id)
    {
        var room = _authCache._roomCache.FirstOrDefault(x => x.Id == id);
        if (room != null)
            return room.RoomName; 
        return null;
    }
    public async Task GetAllSections()
    {
        var sections = await _sectionRepo.GetByExpression(x => x.IsDeleted == false);
        if (sections != null && sections.Any())
            foreach (var section in sections) _authCache._sectionCache.Add(section);
    }
    public string GetSectionName(int id)
    {
        var section = _authCache._sectionCache.FirstOrDefault(x => x.Id == id);
        if (section != null)
            return section.SectionName;
        return null;
    }
    public async Task GetAllAppliances()
    {
        var appliances = await _applianceRepo.GetByExpression(x => x.IsDeleted == false);
        if (appliances != null && appliances.Any())
            foreach (var appliance in appliances) _authCache._applianceCache.Add(appliance);
    }
    public string GetApplianceName(int id)
    {
        var appliance = _authCache._applianceCache.FirstOrDefault(x => x.Id == id);
        if (appliance != null)
            return appliance.ApplianceName; 
        return null;
    }
    public async Task GetAllLights()
    {
        var lights = await _lightRepo.GetByExpression(x => x.IsDeleted == false);
        if (lights != null && lights.Any())
            foreach (var light in lights) _authCache._lightCache.Add(light);
    }
    public string GetLightName(int id)
    {
        var light = _authCache._lightCache.FirstOrDefault(x => x.Id == id);
        if (light != null)
            return light.LightName;
        return null;
    }
    public async Task GetAllCameras()
    {
        var cameras = await _cameraRepo.GetByExpression(x => x.IsDeleted == false);
        if (cameras != null && cameras.Any())
            foreach (var camera in cameras) _authCache._cameraCache.Add(camera);
    }
    public string GetCameraName(int id)
    {
        var camera = _authCache._cameraCache.FirstOrDefault(x => x.Id == id);
        if (camera != null)
            return camera.CameraName; 
        return null;
    }
    public async Task GetAllWindows()
    {
        var windows = await _windowRepo.GetByExpression(x => x.IsDeleted == false);
        if (windows != null && windows.Any())
            foreach (var window in windows) _authCache._windowCache.Add(window);
    }
    public string GetWindowName(int id)
    {
        var window = _authCache._windowCache.FirstOrDefault(x => x.Id == id);
        if (window != null)
            return window.WindowName; 
        return null;
    }
    public async Task GetAllDoors()
    {
        var doors = await _doorRepo.GetByExpression(x => x.IsDeleted == false);
        if (doors != null && doors.Any())
            foreach (var door in doors) _authCache._doorCache.Add(door);
    }
    public string GetDoorName(int id)
    {
        var door = _authCache._doorCache.FirstOrDefault(x => x.Id == id);
        if (door != null)
            return door.DoorName; 
        return null;
    }
    public async Task GetAllActions()
    {
        var actions = await _actionRepo.GetByExpression(x => x.IsDeleted == false);
        if (actions != null && actions.Any())
            foreach (var action in actions) _authCache._actionCache.Add(action);
    }
    public string GetActionName(int id)
    {
        var action = _authCache._actionCache.FirstOrDefault(x => x.Id == id);
        if (action != null)
            return action.ActionName;
        return null;
    }
    public async Task GetAllUtilities()
    {
        var utilities = await _utilityRepo.GetByExpression(x => x.IsDeleted == false);
        if (utilities != null && utilities.Any())
            foreach (var utility in utilities) _authCache._utilityCache.Add(utility);
    }
    public string GetUtilityName(int id)
    {
        var utility = _authCache._utilityCache.FirstOrDefault(x => x.Id == id);
        if (utility != null)
            return utility.UtilityName;
        return null;
    }
    public async Task GetAllExternals()
    {
        var externals = await _externalRepo.GetByExpression(x => x.IsDeleted == false);
        if (externals != null && externals.Any())
            foreach (var external in externals) _authCache._externalCache.Add(external);
    }
    public string GetExternalName(int id)
    {
        var external = _authCache._externalCache.FirstOrDefault(x => x.Id == id);
        if (external != null)
            return external.ExternalName;
        return null;
    }
    public async Task GetAllHardwares()
    {
        var hardwares = await _hardwareRepo.GetByExpression(x => x.IsDeleted == false);
        if (hardwares != null && hardwares.Any())
            foreach (var hardware in hardwares) _authCache._hardwareCache.Add(hardware);
    }
    public string GetHardwareName(int id)
    {
        var hardware = _authCache._hardwareCache.FirstOrDefault(x => x.Id == id);
        if (hardware != null)
            return hardware.HardwareName;
        return null;
    }
    public async Task GetAllContacts()
    {
        var contacts = await _contactRepo.GetByExpression(x => x.IsDeleted == false);
        if (contacts != null && contacts.Any())
            foreach (var contact in contacts) _authCache._contactCache.Add(contact);
    }
    public string GetContactName(int id)
    {
        var contact = _authCache._contactCache.FirstOrDefault(x => x.Id == id);
        if (contact != null)
            return $"{contact.FirstName} {contact.LastName}";
        return null;
    }
    public async Task GetAllContactCategories()
    {
        var contactCategories = await _contactCategoryRepo.GetByExpression(x => x.IsDeleted == false);
        if (contactCategories != null && contactCategories.Any())
            foreach (var category in contactCategories) _authCache._contactCategoryCache.Add(category);
    }
    public string GetContactCategoryName(int id)
    {
        var contactCategory = _authCache._contactCategoryCache.FirstOrDefault(x => x.Id == id);
        if (contactCategory != null)
            return contactCategory.Name;
        return null;
    }
    public async Task RunAllFacilities()
    {
        _authCache._actionCache.Clear();
        await GetAllActions();
        _authCache._applianceCache.Clear();
        await GetAllAppliances();
        _authCache._cameraCache.Clear();
        await GetAllCameras();
        _authCache._lightCache.Clear();
        await GetAllLights();
        _authCache._cache.Clear();
        await GetAllPersons();
        _authCache._roomCache.Clear();
        await GetAllRooms();
        _authCache._sectionCache.Clear();
        await GetAllSections();
        _authCache._doorCache.Clear();
        await GetAllDoors();
        _authCache._externalCache.Clear();
        await GetAllExternals();
        _authCache._hardwareCache.Clear();
        await GetAllHardwares();
        _authCache._windowCache.Clear();
        await GetAllWindows();
        _authCache._contactCategoryCache.Clear();
        await GetAllContactCategories();
        _authCache._contactCache.Clear();
        await GetAllContacts();
        foreach (var role in Enum.GetValues(typeof(Role)).Cast<Role>())
            await _realtimeNotificationService.UpdateAppList("GetAppNamesDetails", role, GetAppNamesIdList(role));
    }
    private async Task<AppNamesDetailList> GetAppNamesIdList(Role role)
    {
        if (role == Role.Owner || role == Role.Wife)
        {
            var dashboard = await GetAppNamesDetails();
            if (dashboard != null)  return dashboard;
        }
        else if (role == Role.Child || role == Role.Relative || role == Role.Visitor)
        {
            var dashboard = await GetAppNamesDetails();
            if (dashboard != null) {
                dashboard.ApplianceIdNames = dashboard.ApplianceIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.CameraIdNames = dashboard.CameraIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.LightIdNames = dashboard.LightIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.WindowIdNames = dashboard.WindowIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.DoorIdNames = dashboard.DoorIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.ExternalIdNames = dashboard.ExternalIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.SectionIdNames = dashboard.SectionIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.PersonIdNames = dashboard.PersonIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.RoomIdNames = dashboard.RoomIdNames.Where(x => x.IsHidden == false).ToList();
                return dashboard;
            }
        }
        return new AppNamesDetailList();
    }
    public bool CompareRole(Role based, Role quote)
    {
        //based is role of last locked user
        //quote is role of user trying to unlock it
        //if (quote <= based) return true;
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
            ApplianceIdNames = _authCache._applianceCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.ApplianceName,
                IsHidden = c.IsHidden,
            }).ToList(),
            CameraIdNames = _authCache._cameraCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.CameraName,
                IsHidden = c.IsHidden,
            }).ToList(),
            LightIdNames = _authCache._lightCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.LightName,
                IsHidden = c.IsHidden,
            }).ToList(),
            WindowIdNames = _authCache._windowCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.WindowName,
                IsHidden = c.IsHidden,
            }).ToList(),
            DoorIdNames = _authCache._doorCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.DoorName,
                IsHidden = c.IsHidden,
            }).ToList(),
            ExternalIdNames = _authCache._externalCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.ExternalName,
                IsHidden = c.IsHidden,
            }).ToList(),
            SectionIdNames = _authCache._sectionCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = c.SectionName,
                IsHidden = c.IsHidden,
            }).ToList(),
            RoomIdNames = _authCache._roomCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericRoomNames
            {
                Id = c.Id,
                Name = c.RoomName,
                IsHidden = c.IsHidden,
                SectionId = c.SectionId ?? null
            }).ToList(),
            PersonIdNames = _authCache._cache
            .Select(c => new GenericNames
            {
                Id = c.Id,
                Name = $"{c.PersonDetails.FirstName} {c.PersonDetails.LastName}",
                IsHidden = c.Disabled,
                ImageUrl = c.PersonDetails.ImageUrl,
            }).ToList(),
            ContactCategoryIdNames = _authCache._contactCategoryCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericIdNames
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList(),
            ActionIdNames = _authCache._actionCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericIdNames
            {
                Id = c.Id,
                Name = c.ActionName,
            }).ToList(),
            UtilityIdNames = _authCache._utilityCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericIdNames
            {
                Id = c.Id,
                Name = c.UtilityName,
            }).ToList(),
            HardwareIdNames = _authCache._hardwareCache.Where(x => x.IsDeleted == false)
            .Select(c => new GenericIdNames
            {
                Id = c.Id,
                Name = c.HardwareName,
            }).ToList(),
            ApplianceType = EnumToList<ApplianceType>(),
            Gender = EnumToList<Gender>(),
            DoorType = EnumToList<DoorType>(),
            FacilityType = EnumToList<FacilityType>(),
            UtilityType = EnumToList<UtilityType>(),
            UtilityComponent = GetAllUtilityComponents(),
            Role = EnumToList<Role>()
        };
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
    public List<UtilityTypeComponentsDto> GetAllUtilityComponents()
    {
        return GetUtilityComponents().Select(x => new UtilityTypeComponentsDto
        {
            UtilityType = new GenericIdNames
            {
                Id = (int)x.Key,
                Name = x.Key.ToString()
            },
            Components = x.Value.Select(component => new GenericIdNames
            {
                Id = (int)component,
                Name = component.ToString()
            }).ToList()

        }).ToList();
    }
    public Dictionary<UtilityType, List<UtilityComponent>> GetUtilityComponents()
    {
        return new Dictionary<UtilityType, List<UtilityComponent>>
        {
            [UtilityType.Electricity] = new()
            {
                UtilityComponent.Generator,
                UtilityComponent.SolarPanel
            },

            [UtilityType.Water] = new()
            {
                UtilityComponent.WaterTank,
                UtilityComponent.WaterPump,
                UtilityComponent.WaterMeter
            },

            [UtilityType.Gas] = new()
            {
                UtilityComponent.GasCylinder
            },

            [UtilityType.HVAC] = new()
            {
                UtilityComponent.AirConditioner,
                UtilityComponent.Heater
            },

            [UtilityType.Emergency] = new()
            {
                UtilityComponent.EmergencyNone,
                UtilityComponent.EmergencyMild,
                UtilityComponent.EmergencySevere,
                UtilityComponent.EmergencyCritical
            },

            [UtilityType.Telecommunication] = new()
            {
            },

            [UtilityType.Irrigation] = new()
            {
            }
        };
    }
}

