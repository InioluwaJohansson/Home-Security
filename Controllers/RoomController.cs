using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class RoomController : Controller
{
    IRoomControl _roomControl;
    IJWTAuthentication _jwtAuthentication;
    public RoomController(IRoomControl roomControl, IJWTAuthentication jwtAuthentication)
    {
        _roomControl = roomControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateRoom")]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto createRoomDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var room = await _roomControl.CreateRoom(getAuthControlInfoDto, createRoomDto);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }
    [Authorize]
    [HttpPut("UpdateRoom")]
    public async Task<IActionResult> UpdateRoom([FromBody] UpdateRoomDto updateRoomDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var room = await _roomControl.UpdateRoom(getAuthControlInfoDto, updateRoomDto);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }
    [Authorize]
    [HttpPost("GetRoomById")]
    public async Task<IActionResult> GetRoomById(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var room = await _roomControl.GetRoomById(getAuthControlInfoDto, id);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }
    [Authorize]
    [HttpPost("GetAllRoomsBySectionId")]
    public async Task<IActionResult> GetAllRoomsBySectionId(int sectionId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var room = await _roomControl.GetAllRoomsBySectionId(getAuthControlInfoDto, sectionId);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }
    [Authorize]
    [HttpPost("GetAllRoomsByPersonId")]
    public async Task<IActionResult> GetAllRoomsByPersonId()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var room = await _roomControl.GetAllRoomsByPersonId(getAuthControlInfoDto);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }
    [Authorize]
    [HttpPost("GetRoomByRoomName")]
    public async Task<IActionResult> GetRoomByRoomName(string roomName)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var room = await _roomControl.GetRoomByRoomName(getAuthControlInfoDto, roomName);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }
    [Authorize]
    [HttpPost("GetAllRooms")]
    public async Task<IActionResult> GetAllRooms()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var room = await _roomControl.GetAllRooms(getAuthControlInfoDto);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }
    [Authorize]
    [HttpPut("DeleteRoom")]
    public async Task<IActionResult> DeleteRoom(int roomId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var room = await _roomControl.DeleteRoom(getAuthControlInfoDto, roomId);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }
}
