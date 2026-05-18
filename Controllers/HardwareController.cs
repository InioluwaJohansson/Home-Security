using Home_Security.Authentication;
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
    IJWTAuthentication _jwtAuthentication;
    public HardwareController(IHardwareControl hardwareControl, IJWTAuthentication jwtAuthentication)
    {
        _hardwareControl = hardwareControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPut("CreateHardware")]
    public async Task<IActionResult> CreateHardware(CreateHardwareDto createHardwareDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var hardware = await _hardwareControl.CreateHardware(getAuthControlInfoDto, createHardwareDto, user.PersonId);
        if (hardware.Status == true) return Ok(hardware);
        return Ok(hardware);
    }
    [Authorize]
    [HttpGet("UpdateHardware")]
    public async Task<IActionResult> UpdateHardware(UpdateHardwareDto updateHardwareDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var hardware = await _hardwareControl.UpdateHardware(getAuthControlInfoDto, updateHardwareDto, user.PersonId);
        if (hardware.Status == true) return Ok(hardware);
        return Ok(hardware);
    }
    [Authorize]
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var hardware = await _hardwareControl.GetById(getAuthControlInfoDto, id);
        if (hardware.Status == true) return Ok(hardware);
        return Ok(hardware);
    }
    [Authorize]
    [HttpGet("GetAllHardwares")]
    public async Task<IActionResult> GetAllHardwares()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var hardware = await _hardwareControl.GetAllHardwares(getAuthControlInfoDto);
        if (hardware.Status == true)return Ok(hardware);
        
        return Ok(hardware);
    }
}