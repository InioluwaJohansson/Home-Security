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
    public DoorController(IDoorControl doorControl)
    {
        _doorControl = doorControl;
    }
    [Authorize]
    [HttpPost("CreateDoor")]
    public async Task<IActionResult> CreateDoor([FromBody] CreateDoorDto createDoorDto)
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
        };
        var updateDoorDto = new UpdateDoorDto(){Id = id};
        var door = await _doorControl.LockDoor(getAuthControlInfoDto, updateDoorDto);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }
    [Authorize]
    [HttpPost("GetDoorById")]
    public async Task<IActionResult> GetDoorById(int id)
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
        };
        var door = await _doorControl.DeleteDoor(getAuthControlInfoDto, doorId);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }
}
