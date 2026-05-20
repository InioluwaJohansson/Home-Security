using Home_Security.Authentication;
using Home_Security.Interfaces.Controls.Chats;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> CreateChat([FromBody] CreateChatDto createChatDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.CreateChat(getAuthControlInfoDto, createChatDto);
        if (chat.Status == true)
        {
            return Ok(chat);
        }
        return Ok(chat);
    }
    [Authorize]
    [HttpPut("UpdateChat")]
    public async Task<IActionResult> UpdateChat([FromBody] UpdateChatDto updateChatDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.UpdateChat(getAuthControlInfoDto, updateChatDto);
        if (chat.Status == true)
        {
            return Ok(chat);
        }
        return Ok(chat);
    }
    [Authorize]
    [HttpPost("GetChatById")]
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
        if (chat.Status == true)
        {
            return Ok(chat);
        }
        return Ok(chat);
    }
    [Authorize]
    [HttpPost("GetAllChatsBySectionId")]
    public async Task<IActionResult> GetAllChatsBySectionId(int sectionId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.GetAllChatsBySectionId(getAuthControlInfoDto, sectionId);
        if (chat.Status == true)
        {
            return Ok(chat);
        }
        return Ok(chat);
    }
    [Authorize]
    [HttpPost("GetAllChatsByRoomId")]
    public async Task<IActionResult> GetAllChatsByRoomId(int roomId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.GetAllChatsByRoomId(getAuthControlInfoDto, roomId);
        if (chat.Status == true)
        {
            return Ok(chat);
        }
        return Ok(chat);
    }
    [Authorize]
    [HttpPost("GetAllChats")]
    public async Task<IActionResult> GetAllChats()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var chat = await _chatControl.GetAllChats(getAuthControlInfoDto);
        if (chat.Status == true)
        {
            return Ok(chat);
        }
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
        if (chat.Status == true)
        {
            return Ok(chat);
        }
        return Ok(chat);
    }
}
