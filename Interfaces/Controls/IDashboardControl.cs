using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Controls;
public interface IDashboardControl
{
    public Task<DashboardResponseModel> GetDashboardData(GetAuthControlInfoDto getAuthControlInfoDto, int personId);
    public Task<DashboardResponseModel> GetAppNamesIdList(GetAuthControlInfoDto getAuthControlInfoDto);

}
