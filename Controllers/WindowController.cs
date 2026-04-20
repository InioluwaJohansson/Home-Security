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
    public WindowController(IWindowControl windowControl)
    {
        _windowControl = windowControl;
    }
    [Authorize]
    [HttpPost("CreateWindow")]
    public async Task<IActionResult> CreateWindow([FromBody] CreateWindowDto createWindowDto)
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
        };
        var window = await _windowControl.DeleteWindow(getAuthControlInfoDto, windowId);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }
}
