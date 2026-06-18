using Home_Security.Authentication;
using Home_Security.Interfaces.Controls.Chats;
using Home_Security.Models.ChatDTO;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class ChatController : Controller
{
    IChatControl _chatControl;
    IJWTAuthentication _jwtAuthentication;
    public ChatController(IChatControl chatControl, IJWTAuthentication jwtAuthentication)
    {
        _chatControl = chatControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateChat")]
    public async Task<IActionResult> CreateChat(int recipientPersonId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.CreateDirectChat(getAuthControlInfoDto, recipientPersonId);
        return Ok(chat);
    }
    [Authorize]
    [HttpPost("CreateGroupChat")]
    public async Task<IActionResult> CreateGroupChat(CreateGroupChatDto createGroupChatDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.CreateGroupChat(getAuthControlInfoDto, createGroupChatDto);
        return Ok(chat);
    }
    [Authorize]
    [HttpPut("UpdateGroupChat")]
    public async Task<IActionResult> UpdateGroupChat(UpdateGroupChatDto createGroupChatDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.UpdateGroupChat(getAuthControlInfoDto, createGroupChatDto);
        return Ok(chat);
    }
    [Authorize]
    [HttpPut("DeleteChat")]
    public async Task<IActionResult> DeleteChat(int chatId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.DeleteChat(getAuthControlInfoDto, chatId);
        return Ok(chat);
    }
    [Authorize]
    [HttpGet("GetChatById")]
    public async Task<IActionResult> GetChatById(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.GetChatById(getAuthControlInfoDto, id);
        return Ok(chat);
    }
    [Authorize]
    [HttpPost("AddParticipants")]
    public async Task<IActionResult> AddParticipants(int chatId, AddParticipantsDto addParticipantsDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.AddParticipants(getAuthControlInfoDto, chatId, addParticipantsDto);
        return Ok(chat);
    }
    [Authorize]
    [HttpGet("GetMyChats")]
    public async Task<IActionResult> GetMyChats()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.GetMyChats(getAuthControlInfoDto);
        return Ok(chat);
    }
    [Authorize]
    [HttpPut("RemoveParticipant")]
    public async Task<IActionResult> RemoveParticipant(int chatId, int recipientId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.RemoveParticipant(getAuthControlInfoDto, chatId, recipientId);
        return Ok(chat);
    }
    [Authorize]
    [HttpPost("JoinChat")]
    public async Task<IActionResult> JoinChat(int chatId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.JoinChat(getAuthControlInfoDto, chatId);
        return Ok(chat);
    }
    [Authorize]
    [HttpPost("LeaveChat")]
    public async Task<IActionResult> LeaveChat(int chatId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        await _chatControl.LeaveChat(getAuthControlInfoDto, chatId);
        return Ok();
    }
}
