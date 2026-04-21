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
    IPersonRepo _personRepo;
    IRoomRepo _roomRepo;
    ISectionRepo _sectionRepo;
    public AuthControl(IMemoryCache cache, IMemoryCache roomCache, IMemoryCache sectionCache, IPersonRepo personRepo, IRoomRepo roomRepo, ISectionRepo sectionRepo)
    {
        _cache = cache;
        _roomCache = roomCache;
        _sectionCache = sectionCache;
        _personRepo = personRepo;
        _roomRepo = roomRepo;
        _sectionRepo = sectionRepo;
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
