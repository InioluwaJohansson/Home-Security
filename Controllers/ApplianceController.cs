using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class ApplianceController : Controller
{
    IApplianceControl _applianceControl;
    public ApplianceController(IApplianceControl applianceControl)
    {
        _applianceControl = applianceControl;
    }
    [Authorize]
    [HttpPost("CreateAppliance")]
    public async Task<IActionResult> CreateAppliance([FromBody] CreateApplianceDto createApplianceDto)
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
        var appliance = await _applianceControl.CreateAppliance(getAuthControlInfoDto, createApplianceDto);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }
    [Authorize]
    [HttpPut("UpdateAppliance")]
    public async Task<IActionResult> UpdateAppliance([FromBody] UpdateApplianceDto updateApplianceDto)
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
        var appliance = await _applianceControl.UpdateAppliance(getAuthControlInfoDto, updateApplianceDto);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }
    [Authorize]
    [HttpPost("GetApplianceById")]
    public async Task<IActionResult> GetApplianceById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var appliance = await _applianceControl.GetApplianceById(getAuthControlInfoDto, id);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }
    [Authorize]
    [HttpPost("GetAllAppliancesBySectionId")]
    public async Task<IActionResult> GetAllAppliancesBySectionId(int sectionId)
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
        var appliance = await _applianceControl.GetAllAppliancesBySectionId(getAuthControlInfoDto, sectionId);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }
    [Authorize]
    [HttpPost("GetAllAppliancesByRoomId")]
    public async Task<IActionResult> GetAllAppliancesByRoomId(int roomId)
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
        var appliance = await _applianceControl.GetAllAppliancesByRoomId(getAuthControlInfoDto, roomId);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }
    [Authorize]
    [HttpPost("GetAllAppliances")]
    public async Task<IActionResult> GetAllAppliances()
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
        var appliance = await _applianceControl.GetAllAppliances(getAuthControlInfoDto);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }
    [Authorize]
    [HttpPut("DeleteAppliance")]
    public async Task<IActionResult> DeleteAppliance(int applianceId)
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
        var appliance = await _applianceControl.DeleteAppliance(getAuthControlInfoDto, applianceId);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }
}
