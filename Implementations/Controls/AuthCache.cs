using Home_Security.Implementations.Controls;
using Home_Security.Entities;
using Home_Security.Interfaces.Controls;
namespace Home_Security.Implementations.Controls;
public class AuthCache : IAuthCache
{
    public List<Person> Persons { get; } = new();
    public List<Person> _cache { get; } =  new ();
    public List<Room> _roomCache { get; } =  new ();
    public List<Section> _sectionCache { get; } =  new ();
    public List<Appliance> _applianceCache { get; } =  new ();
    public List<Light> _lightCache { get; } =  new ();
    public List<Camera> _cameraCache { get; } =  new ();
    public List<Window> _windowCache { get; } =  new ();
    public List<Door> _doorCache { get; } =  new ();
    public List<Home_Security.Entities.Action> _actionCache { get; } =  new ();
    public List<External> _externalCache { get; } =  new ();
    public List<Contact> _contactCache { get; } =  new ();
    public List<ContactCategory> _contactCategoryCache { get; } =  new ();
    public List<Hardware> _hardwareCache { get; } =  new ();
}