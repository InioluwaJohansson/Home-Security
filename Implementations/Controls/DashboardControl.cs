using BCrypt.Net;
using Home_Security.Authentication;
using Home_Security.Entities.Identity;
using Home_Security.Entities;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Home_Security.Models.Enums;
using System.Collections.Generic;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Controls;
public class DashboardControl : IDashboardControl
{
    IDashboardService _dashboardService;
    IAuthControl _authControl;
    public DashboardControl(IDashboardService dashboardService, IAuthControl authControl)
    {
        _dashboardService = dashboardService;
        _authControl = authControl;
    }
    public async Task<DashBoardResponseModel> GetDashboardData(GetAuthControlInfoDto getAuthControlInfoDto, int personId)
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
            if (dashboard != null) return dashboard;
        }
        return new DashBoardResponseModel()
        {
            Status = false,
        };
    }
    public async Task<AppNamesDetailList> GetAppNamesIdList(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var dashboard = await _authControl.GetAppNamesDetails();
            if (dashboard != null)  return dashboard;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var dashboard = await _authControl.GetAppNamesDetails();
            if (dashboard != null) {
                dashboard.ApplianceIdNames = dashboard.ApplianceIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.CameraIdNames = dashboard.CameraIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.LightIdNames = dashboard.LightIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.WindowIdNames = dashboard.WindowIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.DoorIdNames = dashboard.DoorIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.ExternalIdNames = dashboard.ExternalIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.SectionIdNames = dashboard.SectionIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.PersonIdNames = dashboard.PersonIdNames.Where(x => x.IsHidden == false).ToList();
                dashboard.RoomIdNames = dashboard.RoomIdNames.Where(x => x.IsHidden == false).ToList();
                return dashboard;
            }
        }
        return new AppNamesDetailList();
    }
}
