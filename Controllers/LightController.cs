using Home_Security.Authentication;
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
    IJWTAuthentication _jwtAuthentication;
    public LightController(ILightControl lightControl, IJWTAuthentication jwtAuthentication)
    {
        _lightControl = lightControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateLight")]
    public async Task<IActionResult> CreateLight([FromBody] CreateLightDto createLightDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var light = await _lightControl.DeleteLight(getAuthControlInfoDto, lightId);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
}
