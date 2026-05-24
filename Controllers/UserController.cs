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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var getUser = await _userService.UpdateUserRole(updateUserDto);
        if (getUser.Status == true)
        {
            return Ok(getUser);
        }
        return Ok(getUser);
    }
    [Authorize]
    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword(UpdateUserPasswordDto updateUserPasswordDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var token = await _tokenService.VerifyToken(updateUserPasswordDto.TokenCode);
        if(token.Status == true)
        {
            var getUser = await _userService.ChangePassword(updateUserPasswordDto);
            if (getUser.Status == true)
            {
                return Ok(getUser);
            }
            return Ok(getUser);
        }
        return Ok(token);
    }
    [Authorize]
    [HttpPut("ChangeAuthorizationCode")]
    public async Task<IActionResult> ChangeAuthorizationCode(UpdateUserAuthorizationCode updateUserAuthorizationCode)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var token = await _tokenService.VerifyToken(updateUserAuthorizationCode.TokenCode);
        if(token.Status == true)
        {
            var getUser = await _userService.ChangeAuthorizationCode(updateUserAuthorizationCode);
            if (getUser.Status == true)
            {
                return Ok(getUser);
            }
            return Ok(getUser);
        }
        return Ok(token);
    }
    [Authorize]
    [HttpPost("GenerateToken")]
    public async Task<IActionResult> GenerateToken()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var token = await _tokenControl.GenerateToken(getAuthControlInfoDto);
        if (token.Status == true)
        {
            return Ok(token);
        }
        return Ok(token);
    }
    [Authorize]
    [HttpPost("VerifyCode")]
    public async Task<IActionResult> GenerateToken(string authCode)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        if(authCode = user.AuthorizationCode)
           return Ok(true)
        return Ok(false);
    }
}
