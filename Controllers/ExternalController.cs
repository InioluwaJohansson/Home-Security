using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class ExternalController : Controller
{
    IExternalControl _lightControl;
    public ExternalController(IExternalControl lightControl)
    {
        _lightControl = lightControl;
    }
    [Authorize]
    [HttpPost("CreateExternal")]
    public async Task<IActionResult> CreateExternal([FromBody] CreateExternalDto createExternalDto)
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
        var light = await _lightControl.CreateExternal(getAuthControlInfoDto, createExternalDto);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
    [Authorize]
    [HttpPut("UpdateExternal")]
    public async Task<IActionResult> UpdateExternal([FromBody] UpdateExternalDto updateExternalDto)
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
        var light = await _lightControl.UpdateExternal(getAuthControlInfoDto, updateExternalDto);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
    [Authorize]
    [HttpPost("GetExternalById")]
    public async Task<IActionResult> GetExternalById(int id)
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
        var light = await _lightControl.GetExternalById(getAuthControlInfoDto, id);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
    [Authorize]
    [HttpPost("GetAllExternalsBySectionId")]
    public async Task<IActionResult> GetAllExternalsBySectionId(int sectionId)
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
        var light = await _lightControl.GetAllExternalsBySectionId(getAuthControlInfoDto, sectionId);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
    [Authorize]
    [HttpPost("GetAllExternalsByRoomId")]
    public async Task<IActionResult> GetAllExternalsByRoomId(int roomId)
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
        var light = await _lightControl.GetAllExternalsByRoomId(getAuthControlInfoDto, roomId);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
    [Authorize]
    [HttpPost("GetAllExternals")]
    public async Task<IActionResult> GetAllExternals()
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
        var light = await _lightControl.GetAllExternals(getAuthControlInfoDto);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
    [Authorize]
    [HttpPut("DeleteExternal")]
    public async Task<IActionResult> DeleteExternal(int lightId)
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
        var light = await _lightControl.DeleteExternal(getAuthControlInfoDto, lightId);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
}
