using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class LightController : Controller
{
    ILightControl _lightControl;
    public LightController(ILightControl lightControl)
    {
        _lightControl = lightControl;
    }
    [HttpPost("CreateLight")]
    public async Task<IActionResult> CreateLight([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] CreateLightDto createLightDto)
    {
        var light = await _lightControl.CreateLight(getAuthControlInfoDto, createLightDto);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }

    [HttpPut("UpdateLight")]
    public async Task<IActionResult> UpdateLight([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdateLightDto updateLightDto)
    {
        var light = await _lightControl.UpdateLight(getAuthControlInfoDto, updateLightDto);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }

    [HttpGet("GetLightById")]
    public async Task<IActionResult> GetLightById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var light = await _lightControl.GetLightById(getAuthControlInfoDto, id);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }

    [HttpGet("GetAllLightsBySectionId")]
    public async Task<IActionResult> GetAllLightsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var light = await _lightControl.GetAllLightsBySectionId(getAuthControlInfoDto, sectionId);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }

    [HttpGet("GetAllLightsByRoomId")]
    public async Task<IActionResult> GetAllLightsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId)
    {
        var light = await _lightControl.GetAllLightsByRoomId(getAuthControlInfoDto, roomId);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }

    [HttpGet("GetAllLights")]
    public async Task<IActionResult> GetAllLights(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var light = await _lightControl.GetAllLights(getAuthControlInfoDto);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }

    [HttpPut("DeleteLight")]
    public async Task<IActionResult> DeleteLight(GetAuthControlInfoDto getAuthControlInfoDto, int lightId)
    {
        var light = await _lightControl.DeleteLight(getAuthControlInfoDto, lightId);
        if (light.Status == true)
        {
            return Ok(light);
        }
        return Ok(light);
    }
}
