using BCrypt.Net;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Repositories;
using Home_Security.Models.DTOs;
using Microsoft.Extensions.Caching.Memory;

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
    IPersonRepo _personRepo;
    IRoomRepo _roomRepo;
    ISectionRepo _sectionRepo;
    IApplianceRepo _applianceRepo;
    ILightRepo _lightRepo;
    ICameraRepo _cameraRepo;
    IWindowRepo _windowRepo;
    IDoorRepo _doorRepo;
    IActionRepo _actionRepo;
    IExternalRepo _externalRepo;
    public AuthControl(IMemoryCache cache, IMemoryCache roomCache, IMemoryCache sectionCache, IMemoryCache applianceCache, IMemoryCache lightCache, IMemoryCache cameraCache, IMemoryCache windowCache, IMemoryCache doorCache, IMemoryCache actionCache, IMemoryCache externalCache, IPersonRepo personRepo, IRoomRepo roomRepo, ISectionRepo sectionRepo,IApplianceRepo applianceRepo,ILightRepo lightRepo, ICameraRepo cameraRepo,IWindowRepo windowRepo, IDoorRepo doorRepo,IActionRepo actionRepo, IExternalRepo externalRepo)
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
        var persons = await _personRepo.GetAll();
        if (persons != null && persons.Any())
        {
            foreach (var person in persons)
            {
                var name = $"{person.PersonDetails.FirstName} {person.PersonDetails.LastName}"; 
                _cache.Set(person.Id, name);
            }
        }
    }
    public string GetUserPersonName(int id)
    {
        if (_cache.TryGetValue(id, out string name))
        {
            return name;
        }
        return null;
    }
    public async Task GetAllRooms()
    {
        var rooms = await _roomRepo.GetAll();
        if (rooms != null && rooms.Any())
        {
            foreach (var room in rooms)
            {
                var name = room.RoomName; 
                _roomCache.Set(room.Id, name);
            }
        }
    }
    public string GetRoomName(int id)
    {
        if (_roomCache.TryGetValue(id, out string name))
        {
            return name;
        }
        return null;
    }
    public async Task GetAllSections()
    {
        var sections = await _sectionRepo.GetAll();
        if (sections != null && sections.Any())
        {
            foreach (var section in sections)
            {
                var name = section.SectionName; 
                _sectionCache.Set(section.Id, name);
            }
        }
    }
    public string GetSectionName(int id)
    {
        if (_sectionCache.TryGetValue(id, out string name))
        {
            return name;
        }
        return null;
    }
    public async Task GetAllAppliances()
    {
        var appliances = await _applianceRepo.GetAll();
        if (appliances != null && appliances.Any())
        {
            foreach (var appliance in appliances)
            {
                var name = appliance.ApplianceName; 
                _applianceCache.Set(appliance.Id, name);
            }
        }
    }
    public string GetApplianceName(int id)
    {
        if (_applianceCache.TryGetValue(id, out string name))
        {
            return name;
        }
        return null;
    }
    public async Task GetAllLights()
    {
        var lights = await _lightRepo.GetAll();
        if (lights != null && lights.Any())
        {
            foreach (var light in lights)
            {
                var name = light.LightName; 
                _lightCache.Set(light.Id, name);
            }
        }
    }
    public string GetLightName(int id)
    {
        if (_lightCache.TryGetValue(id, out string name))
        {
            return name;
        }
        return null;
    }
public async Task GetAllCameras()
    {
        var cameras = await _cameraRepo.GetAll();
        if (cameras != null && cameras.Any())
        {
            foreach (var camera in cameras)
            {
                var name = camera.CameraName; 
                _cameraCache.Set(camera.Id, name);
            }
        }
    }
    public string GetCameraName(int id)
    {
        if (_cameraCache.TryGetValue(id, out string name))
        {
            return name;
        }
        return null;
    }
    public async Task GetAllWindows()
    {
        var windows = await _windowRepo.GetAll();
        if (windows != null && windows.Any())
        {
            foreach (var window in windows)
            {
                var name = window.WindowName; 
                _windowCache.Set(window.Id, name);
            }
        }
    }
    public string GetWindowName(int id)
    {
        if (_windowCache.TryGetValue(id, out string name))
        {
            return name;
        }
        return null;
    }
    public async Task GetAllDoors()
    {
        var doors = await _doorRepo.GetAll();
        if (doors != null && doors.Any())
        {
            foreach (var door in doors)
            {
                var name = door.DoorName; 
                _doorCache.Set(door.Id, name);
            }
        }
    }
    public string GetDoorName(int id)
    {
        if (_doorCache.TryGetValue(id, out string name))
        {
            return name;
        }
        return null;
    }
    public async Task GetAllActions()
    {
        var actions = await _actionRepo.GetAll();
        if (actions != null && actions.Any())
        {
            foreach (var action in actions)
            {
                var name = action.ActionName; 
                _actionCache.Set(action.Id, name);
            }
        }
    }
    public string GetActionName(int id)
    {
        if (_doorCache.TryGetValue(id, out string name))
        {
            return name;
        }
        return null;
    }
    public async Task GetAllExternals()
    {
        var externals = await _externalRepo.GetAll();
        if (externals != null && externals.Any())
        {
            foreach (var external in externals)
            {
                var name = external.ExternalName; 
                _actionCache.Set(external.Id, name);
            }
        }
    }
    public string GetExternalName(int id)
    {
        if (_externalCache.TryGetValue(id, out string name))
        {
            return name;
        }
        return null;
    }
    public async Task RunAllFacilities()
    {
        await GetAllActions();
        await GetAllAppliances();
        await GetAllCameras();
        await GetAllLights();
        await GetAllPersons();
        await GetAllRooms();
        await GetAllSections();
        await GetAllDoors();
        await GetAllExternals();
        await GetAllWindows();
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
}
