using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class LightController : Controller
{
    ILightControl _lightControl;
    public LightController(ILightControl lightControl)
    {
        _lightControl = lightControl;
    }
    [Authorize]
    [HttpPost("CreateLight")]
    public async Task<IActionResult> CreateLight([FromBody] CreateLightDto createLightDto)
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
        var light = await _lightControl.CreateLight(getAuthControlInfoDto, createLightDto);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
    [Authorize]
    [HttpPut("UpdateLight")]
    public async Task<IActionResult> UpdateLight([FromBody] UpdateLightDto updateLightDto)
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
        var light = await _lightControl.UpdateLight(getAuthControlInfoDto, updateLightDto);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
    [Authorize]
    [HttpPost("GetLightById")]
    public async Task<IActionResult> GetLightById(int id)
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
        var light = await _lightControl.GetLightById(getAuthControlInfoDto, id);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
    [Authorize]
    [HttpPost("GetAllLightsBySectionId")]
    public async Task<IActionResult> GetAllLightsBySectionId(int sectionId)
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
        var light = await _lightControl.GetAllLightsBySectionId(getAuthControlInfoDto, sectionId);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
    [Authorize]
    [HttpPost("GetAllLightsByRoomId")]
    public async Task<IActionResult> GetAllLightsByRoomId(int roomId)
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
        var light = await _lightControl.GetAllLightsByRoomId(getAuthControlInfoDto, roomId);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
    [Authorize]
    [HttpPost("GetAllLights")]
    public async Task<IActionResult> GetAllLights()
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
        var light = await _lightControl.GetAllLights(getAuthControlInfoDto);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
    [Authorize]
    [HttpPut("DeleteLight")]
    public async Task<IActionResult> DeleteLight(int lightId)
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
        var light = await _lightControl.DeleteLight(getAuthControlInfoDto, lightId);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
}
