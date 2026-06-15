using Home_Security.Authentication;
using Home_Security.Models.ChatDTO;
using Home_Security.Interfaces.Controls.Chats;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class MessageController : Controller
{
    IMessageControl _chatControl;
    IJWTAuthentication _jwtAuthentication;
    public MessageController(IMessageControl chatControl, IJWTAuthentication jwtAuthentication)
    {
        _chatControl = chatControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("SendMessage")]
    public async Task<IActionResult> SendMessage(SendMessageDto sendMessageDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.SendMessage(getAuthControlInfoDto, sendMessageDto);
        return Ok(chat);
    }
    [Authorize]
    [HttpPost("EditMessage")]
    public async Task<IActionResult> EditMessage(EditMessageDto editMessageDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.EditMessage(getAuthControlInfoDto, editMessageDto);
        return Ok(chat);
    }
    [Authorize]
    [HttpGet("GetChatMessages")]
    public async Task<IActionResult> GetChatMessages(int chatId, int page, int pageSize)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.GetChatMessages(getAuthControlInfoDto, chatId, page, pageSize);
        return Ok(chat);
    }
    [Authorize]
    [HttpPost("DeleteMessage")]
    public async Task<IActionResult> DeleteMessage(int messageId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        await _chatControl.DeleteMessage(getAuthControlInfoDto, messageId);
        return Ok();
    }
    [Authorize]
    [HttpGet("GetUnreadCount")]
    public async Task<IActionResult> GetUnreadCount(int chatId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.GetUnreadCount(getAuthControlInfoDto, chatId);
        return Ok(chat);
    }
}
