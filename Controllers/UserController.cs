using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class UserController : Controller
{
    IUserService _userService;
    ITokenControl _tokenControl;
    ITokenService _tokenService;
    IJWTAuthentication _jwtAuthentication;
    public UserController(IUserService userService, ITokenService tokenService, ITokenControl tokenControl, IJWTAuthentication jwtAuthentication)
    {
        _userService = userService;
        _tokenService = tokenService;
        _tokenControl = tokenControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [HttpGet("Login")]
    public async Task<IActionResult> Login(string userName, string password)
    {
        var user = await _userService.Login(userName, password);
        if (user.Status == true)
        {
            user.Token = _jwtAuthentication.GenerateToken(user.Data);
            return Ok(user);
        }
        return Ok(user);
    }
    [Authorize]
    [HttpPut("UpdateUserRole")]
    public async Task<IActionResult> UpdateUserRole(UpdateUserDto updateUserDto)
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
        var user = await _userService.UpdateUserRole(updateUserDto);
        if (user.Status == true)
        {
            return Ok(user);
        }
        return Ok(user);
    }
    [Authorize]
    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword(UpdateUserPasswordDto updateUserPasswordDto)
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
        var user = await _userService.ChangePassword(updateUserPasswordDto);
        if (user.Status == true)
        {
            return Ok(user);
        }
        return Ok(user);
    }
    [Authorize]
    [HttpPut("ChangeAuthorizationCode")]
    public async Task<IActionResult> ChangeAuthorizationCode(UpdateUserAuthorizationCode updateUserAuthorizationCode)
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
        var user = await _userService.ChangeAuthorizationCode(updateUserAuthorizationCode);
        if (user.Status == true)
        {
            return Ok(user);
        }
        return Ok(user);
    }
    [Authorize]
    [HttpPost("GenerateToken")]
    public async Task<IActionResult> GenerateToken()
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
        var token = await _tokenControl.GenerateToken(getAuthControlInfoDto);
        if (token.Status == true)
        {
            return Ok(token);
        }
        return Ok(token);
    }
}
