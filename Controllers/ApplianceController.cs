using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class ApplianceController : Controller
{
    IApplianceControl _applianceControl;
    public ApplianceController(IApplianceControl applianceControl)
    {
        _applianceControl = applianceControl;
    }
    [HttpPost("CreateAppliance")]
    public async Task<IActionResult> CreateAppliance([FromQuery]GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] CreateApplianceDto createApplianceDto)
    {
        var appliance = await _applianceControl.CreateAppliance(getAuthControlInfoDto, createApplianceDto);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }

    [HttpPut("UpdateAppliance")]
    public async Task<IActionResult> UpdateAppliance([FromQuery]GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdateApplianceDto updateApplianceDto)
    {
        var appliance = await _applianceControl.UpdateAppliance(getAuthControlInfoDto, updateApplianceDto);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }

    [HttpGet("GetApplianceById")]
    public async Task<IActionResult> GetApplianceById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var appliance = await _applianceControl.GetApplianceById(getAuthControlInfoDto, id);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }

    [HttpGet("GetAllAppliancesBySectionId")]
    public async Task<IActionResult> GetAllAppliancesBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var appliance = await _applianceControl.GetAllAppliancesBySectionId(getAuthControlInfoDto, sectionId);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }

    [HttpGet("GetAllAppliancesByRoomId")]
    public async Task<IActionResult> GetAllAppliancesByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId)
    {
        var appliance = await _applianceControl.GetAllAppliancesByRoomId(getAuthControlInfoDto, roomId);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }

    [HttpGet("GetAllAppliances")]
    public async Task<IActionResult> GetAllAppliances(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var appliance = await _applianceControl.GetAllAppliances(getAuthControlInfoDto);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }

    [HttpPut("DeleteAppliance")]
    public async Task<IActionResult> DeleteAppliance(GetAuthControlInfoDto getAuthControlInfoDto, int applianceId)
    {
        var appliance = await _applianceControl.DeleteAppliance(getAuthControlInfoDto, applianceId);
        if (appliance.Status == true)
        {
            return Ok(appliance);
        }
        return Ok(appliance);
    }
}
