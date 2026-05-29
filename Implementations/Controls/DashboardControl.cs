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
    public async Task<DashboardResponseModel> GetDashboardData(GetAuthControlInfoDto getAuthControlInfoDto, int personId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var dashboard = await _dashboardService.GetDashboardData(auth.Id);
            if (dashboard != null)  return dashboard;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var dashboard = await _dashboardService.GetDashboardData(auth.Id);
            if (dashboard != null) {
                if(dashboard.Rooms != null)
                {
                    dashboard.Rooms = dashboard.Rooms.Where(x => x.IsHidden == false).ToList();
                }
            }
        }
        return new DashboardResponseModel()
        {
            Status = false,
        };
    }
}
