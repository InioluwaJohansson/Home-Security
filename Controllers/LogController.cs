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
    public LogController(ILogControl logControl)
    {
        _logControl = logControl;
    }
    [Authorize]
    [HttpPost("GetLogById")]
    public async Task<IActionResult> GetLogById(int id)
    {
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
        };
        var log = await _logControl.GetAllLogs(getAuthControlInfoDto);
        if (log.Status == true)
        {
            return Ok(log);
        }
        return Ok(log);
    }
}
