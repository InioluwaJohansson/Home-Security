using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Services;
public interface IDashboardService
{
    public Task<DashboardResponseModel> GetDashboardData(int personId);
}