using Home_Security.Interfaces.Controls;
using Microsoft.Extensions.Caching.Memory;

namespace Home_Security.Cache;

public class UserPersonCacheService
{
    public readonly IMemoryCache _cache;
    IAuthControl _authControl;
    public UserPersonCacheService(IMemoryCache cache, IAuthControl authControl)
    {
        _cache = cache;
        _authControl = authControl;
    }

    public async Task<string?> GetUserName(int id)
    {
        if (_cache.TryGetValue(id, out string name))
        {
            return name;
        }

        var persons = await _authControl.GetAllPersons();

        if (persons == null)
            return null;
        foreach(var person in persons)
        {
            _cache.Set(person.Id, person.Name);
            if (person.Id == id)
                name = person.Name;
        }
        return name;
    }
}