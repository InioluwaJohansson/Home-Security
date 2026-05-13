using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class HardwareController : Controller
{
    IHardwareControl _hardwareControl;
    public HardwareController(IHardwareControl hardwareControl)
    {
        _hardwareControl = hardwareControl;
    }
    [Authorize]
    [HttpPut("CreateHardware")]
    public async Task<IActionResult> CreateHardware(CreateHardwareDto createHardwareDto)
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
        var hardware = await _hardwareControl.CreateHardware(getAuthControlInfoDto, createHardwareDto, loggedInPersonId);
        if (hardware.Status == true) return Ok(hardware);
        return Ok(hardware);
    }
    [Authorize]
    [HttpGet("UpdateHardware")]
    public async Task<IActionResult> UpdateHardware(UpdateHardwareDto updateHardwareDto)
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
        var hardware = await _hardwareControl.UpdateHardware(getAuthControlInfoDto, updateHardwareDto, loggedInPersonId);
        if (hardware.Status == true) return Ok(hardware);
        return Ok(hardware);
    }
    [Authorize]
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int id)
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
        var hardware = await _hardwareControl.GetById(getAuthControlInfoDto, id);
        if (hardware.Status == true) return Ok(hardware);
        return Ok(hardware);
    }
    [Authorize]
    [HttpGet("GetAllHardwares")]
    public async Task<IActionResult> GetAllHardwares()
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
        var hardware = await _hardwareControl.GetAllHardwares(getAuthControlInfoDto);
        if (hardware.Status == true)return Ok(hardware);
        
        return Ok(hardware);
    }
}