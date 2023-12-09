using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class WindowController : Controller
{
    IWindowControl _windowControl;
    public WindowController(IWindowControl windowControl)
    {
        _windowControl = windowControl;
    }
    [HttpPost("CreateWindow")]
    public async Task<IActionResult> CreateWindow([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] CreateWindowDto createWindowDto)
    {
        var window = await _windowControl.CreateWindow(getAuthControlInfoDto, createWindowDto);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }

    [HttpPut("UpdateWindow")]
    public async Task<IActionResult> UpdateWindow([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdateWindowDto updateWindowDto)
    {
        var window = await _windowControl.UpdateWindow(getAuthControlInfoDto, updateWindowDto);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }

    [HttpPut("UnlockWindow")]
    public async Task<IActionResult> UnlockWindow([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdateWindowDto updateWindowDto)
    {
        var window = await _windowControl.UnlockWindow(getAuthControlInfoDto, updateWindowDto);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }

    [HttpPut("LockWindow")]
    public async Task<IActionResult> LockWindow([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdateWindowDto updateWindowDto)
    {
        var window = await _windowControl.LockWindow(getAuthControlInfoDto, updateWindowDto);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }

    [HttpGet("GetWindowById")]
    public async Task<IActionResult> GetWindowById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var window = await _windowControl.GetWindowById(getAuthControlInfoDto, id);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }

    [HttpGet("GetAllWindowsBySectionId")]
    public async Task<IActionResult> GetAllWindowsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var window = await _windowControl.GetAllWindowsBySectionId(getAuthControlInfoDto, sectionId);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }

    [HttpGet("GetAllWindowsByRoomId")]
    public async Task<IActionResult> GetAllWindowsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId)
    {
        var window = await _windowControl.GetAllWindowsByRoomId(getAuthControlInfoDto, roomId);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }

    [HttpGet("GetAllWindows")]
    public async Task<IActionResult> GetAllWindows(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var window = await _windowControl.GetAllWindows(getAuthControlInfoDto);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }

    [HttpPut("DeleteWindow")]
    public async Task<IActionResult> DeleteWindow(GetAuthControlInfoDto getAuthControlInfoDto, int windowId)
    {
        var window = await _windowControl.DeleteWindow(getAuthControlInfoDto, windowId);
        if (window.Status == true)
        {
            return Ok(window);
        }
        return Ok(window);
    }
}
