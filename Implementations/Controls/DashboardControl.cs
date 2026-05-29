using BCrypt.Net;
using Home_Security.Authentication;
using Home_Security.Entities.Identity;
using Home_Security.Entities;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Repositories;
using Home_Security.Models.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Home_Security.Models.Enums;
using System.Collections.Generic;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Controls;
public class DashboardControl : IDashboardControl
{
    IDashboardService _dashboardService;
    IJWTAuthentication _jwtAuthentication;
    public DashboardControl(IDashboardService dashboardService, IJWTAuthentication jwtAuthentication)
    {
        _dashboardService = dashboardService;
        _jwtAuthentication = jwtAuthentication;
    }
    public async Task<DashboardResponseModel> GetDashboardDataAsync(string token)
    {
        var personId = _jwtAuthentication.GetPersonIdFromToken(token);
        var dashboardData = await _dashboardService.GetDashboardDataAsync(personId);
        return dashboardData;
    }
}
