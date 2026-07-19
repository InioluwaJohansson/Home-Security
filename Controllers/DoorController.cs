using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class DoorController : Controller
{
    IDoorControl _doorControl;
    IJWTAuthentication _jwtAuthentication;
    public DoorController(IDoorControl doorControl, IJWTAuthentication jwtAuthentication)
    {
        _doorControl = doorControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateDoor")]
    public async Task<IActionResult> CreateDoor([FromBody] CreateDoorDto createDoorDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var door = await _doorControl.CreateDoor(getAuthControlInfoDto, createDoorDto);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }
    [Authorize]
    [HttpPut("UpdateDoor")]
    public async Task<IActionResult> UpdateDoor([FromBody] UpdateDoorDto updateDoorDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var door = await _doorControl.UpdateDoor(getAuthControlInfoDto, updateDoorDto);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }
    [Authorize]
    [HttpPut("UnlockDoor")]
    public async Task<IActionResult> UnlockDoor(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var updateDoorDto = new UpdateDoorDto(){Id = id,};
        var door = await _doorControl.UnlockDoor(getAuthControlInfoDto, updateDoorDto);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }
    [Authorize]
    [HttpPut("LockDoor")]
    public async Task<IActionResult> LockDoor(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var updateDoorDto = new UpdateDoorDto(){Id = id};
        var door = await _doorControl.LockDoor(getAuthControlInfoDto, updateDoorDto);
        if (door.Status == true)
        {
            Console.WriteLine("Door Message: " + door.Message);
            return Ok(door);
        }
        return Ok(door);
    }
    [Authorize]
    [HttpPost("GetDoorById")]
    public async Task<IActionResult> GetDoorById(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var door = await _doorControl.GetDoorById(getAuthControlInfoDto, id);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }
    [Authorize]
    [HttpPost("GetAllDoorsBySectionId")]
    public async Task<IActionResult> GetAllDoorsBySectionId(int sectionId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var door = await _doorControl.GetAllDoorsBySectionId(getAuthControlInfoDto, sectionId);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }
    [Authorize]
    [HttpPost("GetAllDoorsByRoomId")]
    public async Task<IActionResult> GetAllDoorsByRoomId(int roomId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var door = await _doorControl.GetAllDoorsByRoomId(getAuthControlInfoDto, roomId);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }
    [Authorize]
    [HttpPost("GetAllDoorsByDoorType")]
    public async Task<IActionResult> GetAllDoorsByDoorType(DoorType doorType)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var door = await _doorControl.GetAllDoorsByDoorType(getAuthControlInfoDto, doorType);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }
    [Authorize]
    [HttpPost("GetAllDoors")]
    public async Task<IActionResult> GetAllDoors()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var door = await _doorControl.GetAllDoors(getAuthControlInfoDto);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }
    [Authorize]
    [HttpPut("DeleteDoor")]
    public async Task<IActionResult> DeleteDoor(int doorId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var door = await _doorControl.DeleteDoor(getAuthControlInfoDto, doorId);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }
}
