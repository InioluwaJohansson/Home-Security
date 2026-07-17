using Home_Security.Authentication;
using Home_Security.Interfaces.Controls.Calls;
using Home_Security.Models.CallDTO;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class CallController : Controller
{
    ICallControl _callControl;
    IJWTAuthentication _jwtAuthentication;
    public CallController(ICallControl callControl, IJWTAuthentication jwtAuthentication)
    {
        _callControl = callControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("StartCall")]
    public async Task<IActionResult> CreateChat(StartCallDto startCallDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _callControl.StartCall(getAuthControlInfoDto, startCallDto);
        return Ok(chat);
    }
    [Authorize]
    [HttpPost("EndCall")]
    public async Task<IActionResult> EndCall(int callId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var call = await _callControl.EndCall(getAuthControlInfoDto, callId);
        return Ok(call);
    }
}