using Home_Security.Entities;
using Home_Security.Interfaces.Controls.Defaults;
using Home_Security.Interfaces.Repositories;

namespace Home_Security.Implementations.Controls.Defaults;
public partial class ObjectDefault : IObjectDefault
{
    IApplianceRepo _applianceRepo;
    ICameraRepo _cameraRepo;
    IContactCategoryRepo _contactCategoryRepo;
    IContactRepo _contactRepo;
    IDoorRepo _doorRepo;
    ILightRepo _lightRepo;
    IPersonRepo _personRepo;
    IRoomRepo _roomRepo;
    ISectionRepo _sectionRepo;
    IWindowRepo _windowRepo;

    public ObjectDefault(IApplianceRepo applianceRepo, ICameraRepo cameraRepo, IContactCategoryRepo contactCategoryRepo, IContactRepo contactRepo, IDoorRepo doorRepo, ILightRepo lightRepo, IPersonRepo personRepo, IRoomRepo roomRepo, ISectionRepo sectionRepo, IWindowRepo windowRepo)
    {
        _applianceRepo = applianceRepo;
        _cameraRepo = cameraRepo;
        _contactCategoryRepo = contactCategoryRepo;
        _contactRepo = contactRepo;
        _doorRepo = doorRepo;
        _lightRepo = lightRepo;
        _personRepo = personRepo;
        _roomRepo = roomRepo;
        _sectionRepo = sectionRepo;
        _windowRepo = windowRepo;
    }
    public async Task<string> ApplianceName(int id)
    {
        var appliance = await _applianceRepo.Get(x => x.Id == id);
        if (appliance != null)
        {
            return appliance.ApplianceName;
        }
        return null;
    }
    public async Task<string> CameraName(int id)
    {
        var camera = await _cameraRepo.Get(x => x.Id == id);
        if (camera != null)
        {
            return camera.CameraName;
        }
        return null;
    }
    public async Task<string> ContactCategoryName(int id)
    {
        var contactCategory = await _contactCategoryRepo.Get(x => x.Id == id);
        if (contactCategory != null)
        {
            return contactCategory.Name;
        }
        return null;
    }
    public async Task<string> ContactName(int id)
    {
        var contact = await _contactRepo.Get(x => x.Id == id);
        if (contact != null)
        {
            return $"{contact.LastName} {contact.FirstName}";
        }
        return null;
    }
    public async Task<string> DoorName(int id)
    {
        var door = await _doorRepo.Get(x => x.Id == id);
        if (door != null)
        {
            return door.DoorName;
        }
        return null;
    }
    public async Task<string> LightName(int id)
    {
        var light = await _lightRepo.Get(x => x.Id == id);
        if (light != null)
        {
            return light.LightName;
        }
        return null;
    }
    public async Task<string> PersonName(int id)
    {
        var person = await _personRepo.GetById(id);
        if (person != null)
        {
            return $"{person.PersonDetails.LastName} {person.PersonDetails.FirstName}";
        }
        return null;
    }
    public async Task<string> RoomName(int id)
    {
        var room = await _roomRepo.Get(x => x.Id == id);
        if (room != null)
        {
            return room.RoomName;
        }
        return null;
    }
    public async Task<string> SectionName(int id)
    {
        var section = await _sectionRepo.Get(x => x.Id == id);
        if (section != null)
        {
            return section.SectionName;
        }
        return null;
    }
    public async Task<string> WindowName(int id)
    {
        var window = await _windowRepo.Get(x => x.Id == id);
        if (window != null)
        {
            return window.WindowName;
        }
        return null;
    }
}
