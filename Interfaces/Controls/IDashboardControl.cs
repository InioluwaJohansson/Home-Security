using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Controls;
public interface IDashboardControl
{
    public Task<DashBoardResponseModel> GetDashboardData(GetAuthControlInfoDto getAuthControlInfoDto, int personId);
    public Task<AppNamesDetailList> GetAppNamesIdList(GetAuthControlInfoDto getAuthControlInfoDto);
}
