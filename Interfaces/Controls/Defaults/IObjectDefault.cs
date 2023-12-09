namespace Home_Security.Interfaces.Controls.Defaults;
public interface IObjectDefault
{
    public Task<string> ApplianceName(int id);
    public Task<string> CameraName(int id);
    public Task<string> ContactCategoryName(int id);
    public Task<string> ContactName(int id);
    public Task<string> DoorName(int id);
    public Task<string> LightName(int id);
    public Task<string> PersonName(int id);
    public Task<string> RoomName(int id);
    public Task<string> SectionName(int id);
    public Task<string> WindowName(int id);
}
