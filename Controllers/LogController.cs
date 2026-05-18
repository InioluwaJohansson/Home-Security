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
    ILogControl _logControl;
    IJWTAuthentication _jwtAuthentication;
    public LogController(ILogControl logControl, JWTAuthentication jwtAuthentication)
    {
        _logControl = logControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("GetLogById")]
    public async Task<IActionResult> GetLogById(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var log = await _logControl.GetLogById(getAuthControlInfoDto, id);
        if (log.Status == true)
        {
            return Ok(log);
        }
        return Ok(log);
    }
    [Authorize]
    [HttpPost("GetAllLogsByPersonId")]
    public async Task<IActionResult> GetAllLogsBySectionId(int personId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var log = await _logControl.GetAllLogsByPersonId(getAuthControlInfoDto, personId);
        if (log.Status == true)
        {
            return Ok(log);
        }
        return Ok(log);
    }
    [Authorize]
    [HttpPost("GetAllLogsByActionType")]
    public async Task<IActionResult> GetAllLogsByActionType(string actionType)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var log = await _logControl.GetAllLogsByActionType(getAuthControlInfoDto, actionType);
        if (log.Status == true)
        {
            return Ok(log);
        }
        return Ok(log);
    }
    [Authorize]
    //Ned review due to fetching of large sumps of data
    [HttpPost("GetAllLogs")]
    public async Task<IActionResult> GetAllLogs()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var log = await _logControl.GetAllLogs(getAuthControlInfoDto);
        if (log.Status == true)
        {
            return Ok(log);
        }
        return Ok(log);
    }
}
