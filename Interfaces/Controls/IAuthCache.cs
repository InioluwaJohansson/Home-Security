using Home_Security.Entities;
namespace Home_Security.Interfaces.Controls;
public interface IAuthCache
{
    public List<Person> _cache { get; }
    public List<Room> _roomCache { get; }
    public List<Section> _sectionCache { get; }
    public List<Appliance> _applianceCache { get; }
    public List<Light> _lightCache { get; }
    public List<Camera> _cameraCache { get; }
    public List<Window> _windowCache { get; }
    public List<Door> _doorCache { get; }
    public List<Home_Security.Entities.Action> _actionCache { get; }
    public List<External> _externalCache { get; }
    public List<Contact> _contactCache { get; }
    public List<ContactCategory> _contactCategoryCache { get; }
    public List<Hardware> _hardwareCache { get; }
}