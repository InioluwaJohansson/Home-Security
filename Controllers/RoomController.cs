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
    public RoomController(IRoomControl roomControl)
    {
        _roomControl = roomControl;
    }
    [Authorize]
    [HttpPost("CreateRoom")]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto createRoomDto)
    {
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
        };
        var room = await _roomControl.GetAllRoomsBySectionId(getAuthControlInfoDto, sectionId);
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
        };
        var room = await _roomControl.DeleteRoom(getAuthControlInfoDto, roomId);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }
}
