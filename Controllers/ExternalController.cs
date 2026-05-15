using Home_Security.Authentication;
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
    IJWTAuthentication _jWTAuthentication;
    public ExternalController(IExternalControl lightControl, IJWTAuthentication jwtAuthentication)
    {
        _lightControl = lightControl;
        _jWTAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateExternal")]
    public async Task<IActionResult> CreateExternal([FromBody] CreateExternalDto createExternalDto)
    {
        var user = _jWTAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jWTAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jWTAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jWTAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jWTAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jWTAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jWTAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var light = await _lightControl.DeleteExternal(getAuthControlInfoDto, lightId);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
}
