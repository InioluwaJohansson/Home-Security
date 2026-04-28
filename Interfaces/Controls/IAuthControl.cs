using Home_Security.Entities.Identity;
using Home_Security.Implementations.Controls;
using Home_Security.Interfaces.Controls.Defaults;
using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Controls;
public interface IAuthControl
{
    public Task<GetAuthControlDto> GetAuthDetails(int personId, string authorizationCode);
    public Task<GetAuthControlDto> GetPersonDetails(int personId);
    public Task GetAllPersons();
    public string GetUserPersonName(int id);
    public Task GetAllRooms();
    public string GetRoomName(int id);
    public Task GetAllSections();
    public string GetSectionName(int id);
    public Task GetAllAppliances();
    public string GetApplianceName(int id);
    public Task GetAllLights();
    public string GetLightName(int id);
    public Task GetAllCameras();
    public string GetCameraName(int id);
    public Task GetAllWindows();
    public string GetWindowName(int id);
    public Task GetAllDoors();
    public string GetDoorName(int id);
    public bool CompareRole(Role based, Role quote);
    public BaseResponse AuthFaliure();
    public CreateLogDto CreateLog(int personId);
}
