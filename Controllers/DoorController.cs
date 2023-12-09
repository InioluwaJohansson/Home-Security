using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class DoorController : Controller
{
    IDoorControl _doorControl;
    public DoorController(IDoorControl doorControl)
    {
        _doorControl = doorControl;
    }
    [HttpPost("CreateDoor")]
    public async Task<IActionResult> CreateDoor([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] CreateDoorDto createDoorDto)
    {
        var door = await _doorControl.CreateDoor(getAuthControlInfoDto, createDoorDto);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }

    [HttpPut("UpdateDoor")]
    public async Task<IActionResult> UpdateDoor([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdateDoorDto updateDoorDto)
    {
        var door = await _doorControl.UpdateDoor(getAuthControlInfoDto, updateDoorDto);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }

    [HttpPut("UnlockDoor")]
    public async Task<IActionResult> UnlockDoor([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdateDoorDto updateDoorDto)
    {
        var door = await _doorControl.UnlockDoor(getAuthControlInfoDto, updateDoorDto);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }

    [HttpPut("LockDoor")]
    public async Task<IActionResult> LockDoor([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdateDoorDto updateDoorDto)
    {
        var door = await _doorControl.LockDoor(getAuthControlInfoDto, updateDoorDto);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }

    [HttpGet("GetDoorById")]
    public async Task<IActionResult> GetDoorById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var door = await _doorControl.GetDoorById(getAuthControlInfoDto, id);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }

    [HttpGet("GetAllDoorsBySectionId")]
    public async Task<IActionResult> GetAllDoorsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var door = await _doorControl.GetAllDoorsBySectionId(getAuthControlInfoDto, sectionId);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }

    [HttpGet("GetAllDoorsByRoomId")]
    public async Task<IActionResult> GetAllDoorsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId)
    {
        var door = await _doorControl.GetAllDoorsByRoomId(getAuthControlInfoDto, roomId);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }

    [HttpGet("GetAllDoorsByDoorType")]
    public async Task<IActionResult> GetAllDoorsByDoorType(GetAuthControlInfoDto getAuthControlInfoDto, DoorType doorType)
    {
        var door = await _doorControl.GetAllDoorsByDoorType(getAuthControlInfoDto, doorType);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }

    [HttpGet("GetAllDoors")]
    public async Task<IActionResult> GetAllDoors(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var door = await _doorControl.GetAllDoors(getAuthControlInfoDto);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }

    [HttpPut("DeleteDoor")]
    public async Task<IActionResult> DeleteDoor(GetAuthControlInfoDto getAuthControlInfoDto, int doorId)
    {
        var door = await _doorControl.DeleteDoor(getAuthControlInfoDto, doorId);
        if (door.Status == true)
        {
            return Ok(door);
        }
        return Ok(door);
    }
}
