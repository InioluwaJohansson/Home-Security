using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class UserController : Controller
{
    IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet("Login")]
    public async Task<IActionResult> Login(string userName, string password)
    {
        var user = await _userService.Login(userName, password);
        if (user.Status == true)
        {
            return Ok(user);
        }
        return Ok(user);
    }

    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
    {
        var user = await _userService.UpdateUser(updateUserDto);
        if (user.Status == true)
        {
            return Ok(user);
        }
        return Ok(user);
    }

    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword(UpdateUserPasswordDto updateUserPasswordDto)
    {
        var user = await _userService.ChangePassword(updateUserPasswordDto);
        if (user.Status == true)
        {
            return Ok(user);
        }
        return Ok(user);
    }

    [HttpPut("ChangeAuthorizationCode")]
    public async Task<IActionResult> ChangeAuthorizationCode(UpdateUserAuthorizationCode updateUserAuthorizationCode)
    {
        var user = await _userService.ChangeAuthorizationCode(updateUserAuthorizationCode);
        if (user.Status == true)
        {
            return Ok(user);
        }
        return Ok(user);
    }
}
