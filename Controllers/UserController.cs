using BCrypt.Net;
﻿using Home_Security.Authentication;
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
    IAuthControl _authControl;
    ITokenService _tokenService;
    IJWTAuthentication _jwtAuthentication;
    public UserController(IUserService userService, ITokenService tokenService, ITokenControl tokenControl, IAuthControl authControl, IJWTAuthentication jwtAuthentication)
    {
        _userService = userService;
        _tokenService = tokenService;
        _tokenControl = tokenControl;
        _authControl = authControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [HttpGet("Login")]
    public async Task<IActionResult> Login(string userName, string password)
    {
        var user = await _userService.Login(userName, password);
        if (user.Status == true)
            return Ok(user);
        return Ok(user);
    }
    [Authorize]
    [HttpGet("NFIDLogin")]
    public async Task<IActionResult> NFIDLogin(string token, string NFIDCode)
    {
        if(_jwtAuthentication.ValidateHardwareToken(token) == true)
        {
            var user = await _userService.NFIDLogin(token, NFIDCode);
            if (user.Status == true)
                return Ok(user);            
        }
        return Unauthorized();
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
    [HttpPost("VerifyAuthCode")]
    public async Task<IActionResult> VerifyAuthCode(string authCode)
    {
        Console.WriteLine("AuthCode: " + authCode);
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        if (user != null && BCrypt.Net.BCrypt.EnhancedVerify(authCode, user.AuthorizationCode)){
        Console.WriteLine("Hashed: Yes " + user.AuthorizationCode + " verified!");
            Console.WriteLine("AuthCode: Yes " + authCode + " verified!");
           return Ok(true);
        }
        return Ok(false);
    }
    [Authorize]
    [HttpPost("RefreshLoginToken")]
    public async Task<IActionResult> RefreshLoginToken()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        if (user != null){
            var data = new UserLoginResponse()
            {
                Data = new GetUserDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    RoleName = user.Role.ToString(),
                    Role = user.Role,
                    AuthorizationCode = user.AuthorizationCode,
                },
                Message = "Login Successful",
                Status = true,
            };
            data.Token = _jwtAuthentication.GenerateToken(data.Data);
            data.Data.AuthorizationCode = "";
            return Ok(user);
        }
        return Unauthorized();
    }
}
