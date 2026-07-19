using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Implementations.Controls;
using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Controls;
public interface IAuthControl
{
    public Task<GetAuthControlDto> GetAuthDetails(int personId, string authorizationCode);
    public Task<GetAuthControlDto> GetPersonDetails(int personId);
    public Person GetPersonById(int personId);
    public string GetUserPersonName(int id);
    public string GetRoomName(int id);
    public string GetSectionName(int id);
    public string GetApplianceName(int id);
    public string GetLightName(int id);
    public string GetCameraName(int id);
    public string GetWindowName(int id);
    public string GetDoorName(int id);
    public string GetExternalName(int id);
    public string GetContactName(int id);
    public string GetContactCategoryName(int id);
    public string GetActionName(int id);
    public Task RunAllFacilities();
    public bool CompareRole(Role based, Role quote);
    public BaseResponse AuthFaliure();
    public CreateLogDto CreateLog(int personId);
    public Task<AppNamesDetailList> GetAppNamesDetails();
}
