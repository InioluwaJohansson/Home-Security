using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class RoomController : Controller
{
    IRoomControl _roomControl;
    public RoomController(IRoomControl roomControl)
    {
        _roomControl = roomControl;
    }
    [HttpPost("CreateRoom")]
    public async Task<IActionResult> CreateRoom([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] CreateRoomDto createRoomDto)
    {
        var room = await _roomControl.CreateRoom(getAuthControlInfoDto, createRoomDto);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }

    [HttpPut("UpdateRoom")]
    public async Task<IActionResult> UpdateRoom([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdateRoomDto updateRoomDto)
    {
        var room = await _roomControl.UpdateRoom(getAuthControlInfoDto, updateRoomDto);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }

    [HttpGet("GetRoomById")]
    public async Task<IActionResult> GetRoomById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var room = await _roomControl.GetRoomById(getAuthControlInfoDto, id);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }

    [HttpGet("GetAllRoomsBySectionId")]
    public async Task<IActionResult> GetAllRoomsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var room = await _roomControl.GetAllRoomsBySectionId(getAuthControlInfoDto, sectionId);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }

    [HttpGet("GetRoomByRoomName")]
    public async Task<IActionResult> GetRoomByRoomName(GetAuthControlInfoDto getAuthControlInfoDto, string roomName)
    {
        var room = await _roomControl.GetRoomByRoomName(getAuthControlInfoDto, roomName);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }

    [HttpGet("GetAllRooms")]
    public async Task<IActionResult> GetAllRooms(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var room = await _roomControl.GetAllRooms(getAuthControlInfoDto);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }

    [HttpPut("DeleteRoom")]
    public async Task<IActionResult> DeleteRoom(GetAuthControlInfoDto getAuthControlInfoDto, int roomId)
    {
        var room = await _roomControl.DeleteRoom(getAuthControlInfoDto, roomId);
        if (room.Status == true)
        {
            return Ok(room);
        }
        return Ok(room);
    }
}
