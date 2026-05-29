using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Controls;
public interface IDashboardControl
{
    public Task<DashboardResponseModel> GetDashboardDataAsync(string token);
}
