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
    public async Task<IActionResult> StartCall(StartCallDto startCallDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var call = await _callControl.StartCall(getAuthControlInfoDto, startCallDto);
        return Ok(call);
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
    [Authorize]
    [HttpPost("AcceptCall")]
    public async Task<IActionResult> AcceptCall(int callId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var call = await _callControl.AcceptCall(getAuthControlInfoDto, callId);
        return Ok(call);
    }
    [Authorize]
    [HttpPost("RejectCall")]
    public async Task<IActionResult> RejectCall(int callId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var call = await _callControl.RejectCall(getAuthControlInfoDto, callId);
        return Ok(call);
    }
    [Authorize]
    [HttpGet("GetCallLogs")]
    public async Task<IActionResult> GetCallLogs(int page, int pageSize)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var call = await _callControl.GetCallLogsAsync(getAuthControlInfoDto, page, pageSize);
        return Ok(call);
    }
}