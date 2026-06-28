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
    IMessageControl _messageControl;
    IJWTAuthentication _jwtAuthentication;
    public MessageController(IMessageControl messageControl, IJWTAuthentication jwtAuthentication)
    {
        _messageControl = messageControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("SendMessage")]
    public async Task<IActionResult> SendMessage([FromForm] SendMessageDto sendMessageDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var message = await _messageControl.SendMessage(getAuthControlInfoDto, sendMessageDto);
        return Ok(message);
    }
    [Authorize]
    [HttpPost("SendMessageChatId")]
    public async Task<IActionResult> SendMessage([FromForm] SendMessageDto sendMessageDto, int chatId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        sendMessageDto.ChatId = chatId;
        var message = await _messageControl.SendMessage(getAuthControlInfoDto, sendMessageDto);
        return Ok(message);
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
        var message = await _messageControl.EditMessage(getAuthControlInfoDto, editMessageDto);
        return Ok(message);
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
        var message = await _messageControl.GetChatMessages(getAuthControlInfoDto, chatId, page, pageSize);
        return Ok(message);
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
        await _messageControl.DeleteMessage(getAuthControlInfoDto, messageId);
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
        var message = await _messageControl.GetUnreadCount(getAuthControlInfoDto, chatId);
        return Ok(message);
    }
    [Authorize]
    [HttpPost("Typing")]
    public async Task<IActionResult> Typing(int chatId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        await _messageControl.Typing(getAuthControlInfoDto, chatId);
        return Ok();
    }
}
