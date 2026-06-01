using Home_Security.Authentication;
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
    IJWTAuthentication _jwtAuthentication;
    public ApplianceController(IApplianceControl applianceControl, IJWTAuthentication jwtAuthentication)
    {
        _applianceControl = applianceControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateAppliance")]
    public async Task<IActionResult> CreateAppliance([FromBody] CreateApplianceDto createApplianceDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        Console.WriteLine(getAuthControlInfoDto.PersonId + " " + getAuthControlInfoDto.AuthorizationCode);
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var appliance = await _applianceControl.DeleteAppliance(getAuthControlInfoDto, applianceId);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }
}
