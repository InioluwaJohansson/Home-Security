using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class LogController : Controller
{
    IJWTAuthentication _jwtAuthentication;
    ILogControl _logControl;
    public LogController(ILogControl logControl, IJWTAuthentication jwtAuthentication)
    {
        _logControl = logControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("GetAllLogsByPersonId")]
    public async Task<IActionResult> GetAllLogsByPersonId(int personId, int page = 1, int pageSize = 50)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var log = await _logControl.GetAllLogsByPersonId(getAuthControlInfoDto, personId, page, pageSize);
        if (log.Status == true)
        {
            return Ok(log);
        }
        return Ok(log);
    }
    [Authorize]
    [HttpPost("GetAllLogsByDate")]
    public async Task<IActionResult> GetAllLogsByDate(DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 50)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var log = await _logControl.GetAllLogsByDate(getAuthControlInfoDto, startDate, endDate, page, pageSize);
        if (log.Status == true)
        {
            return Ok(log);
        }
        return Ok(log);
    }
    [Authorize]
    [HttpPost("GetAllLogs")]
    public async Task<IActionResult> GetAllLogs(int page = 1, int pageSize = 50)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var log = await _logControl.GetAllLogs(getAuthControlInfoDto, page, pageSize);
        if (log.Status == true)
        {
            return Ok(log);
        }
        return Ok(log);
    }
}
