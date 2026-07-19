using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class DashboardController : Controller
{
    IDashboardControl _dashboardControl;
    IJWTAuthentication _jwtAuthentication;
    public DashboardController(IDashboardControl dashboardControl, IJWTAuthentication jwtAuthentication)
    {
        _dashboardControl = dashboardControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("GetDashboardData")]
    public async Task<IActionResult> GetDashboardData()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var dashboard = await _dashboardControl.GetDashboardData(getAuthControlInfoDto, user.PersonId);
        if (dashboard.Status)
            return Ok(dashboard);
        return Unauthorized();
    }
    [Authorize]
    [HttpPost("GetAppNamesIdList")]
    public async Task<IActionResult> GetAppNamesIdList()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var dashboard = await _dashboardControl.GetAppNamesIdList(getAuthControlInfoDto);
            return Ok(dashboard);
    }
}