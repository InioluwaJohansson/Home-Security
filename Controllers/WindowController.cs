using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class WindowController : Controller
{
    IWindowControl _windowControl;
    IJWTAuthentication _jwtAuthentication;
    public WindowController(IWindowControl windowControl, IJWTAuthentication jwtAuthentication)
    {
        _windowControl = windowControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateWindow")]
    public async Task<IActionResult> CreateWindow([FromBody] CreateWindowDto createWindowDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var window = await _windowControl.CreateWindow(getAuthControlInfoDto, createWindowDto);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }
    [Authorize]
    [HttpPut("UpdateWindow")]
    public async Task<IActionResult> UpdateWindow([FromBody] UpdateWindowDto updateWindowDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var window = await _windowControl.UpdateWindow(getAuthControlInfoDto, updateWindowDto);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }
    [Authorize]
    [HttpPut("UnlockWindow")]
    public async Task<IActionResult> UnlockWindow(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var updateWindowDto = new UpdateWindowDto(){Id = id};
        var window = await _windowControl.UnlockWindow(getAuthControlInfoDto, updateWindowDto);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }
    [Authorize]
    [HttpPut("LockWindow")]
    public async Task<IActionResult> LockWindow(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var updateWindowDto = new UpdateWindowDto(){Id = id};
        var window = await _windowControl.LockWindow(getAuthControlInfoDto, updateWindowDto);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }
    [Authorize]
    [HttpPost("GetWindowById")]
    public async Task<IActionResult> GetWindowById(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var window = await _windowControl.GetWindowById(getAuthControlInfoDto, id);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }
    [Authorize]
    [HttpPost("GetAllWindowsBySectionId")]
    public async Task<IActionResult> GetAllWindowsBySectionId(int sectionId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var window = await _windowControl.GetAllWindowsBySectionId(getAuthControlInfoDto, sectionId);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }
    [Authorize]
    [HttpPost("GetAllWindowsByRoomId")]
    public async Task<IActionResult> GetAllWindowsByRoomId(int roomId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var window = await _windowControl.GetAllWindowsByRoomId(getAuthControlInfoDto, roomId);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }
    [Authorize]
    [HttpPost("GetAllWindows")]
    public async Task<IActionResult> GetAllWindows()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var window = await _windowControl.GetAllWindows(getAuthControlInfoDto);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }
    [Authorize]
    [HttpPut("DeleteWindow")]
    public async Task<IActionResult> DeleteWindow(int windowId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var window = await _windowControl.DeleteWindow(getAuthControlInfoDto, windowId);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }
}
