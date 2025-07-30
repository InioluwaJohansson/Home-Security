using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class HardwareController : Controller
{
    IHardwareControl _hardwareControl;
    public HardwareController(IHardwareControl hardwareControl)
    {
        _hardwareControl = hardwareControl;
    }
    [HttpPut("UpdateHardwareServices")]
    public async Task<IActionResult> UpdateHaUpdateHardwareServicesrdware(UpdateHardwareDto updatehardwareDto)
    {
        var hardware = await _hardwareControl.UpdateHardwareServices(updatehardwareDto);
        if (hardware.Status == true)
        {
            return Ok(hardware);
        }
        return Ok(hardware);
    }

    [HttpGet("GetHardwareServices")]
    public async Task<IActionResult> GetHardwareServices(UpdateHardwareResponseIds updateHardwareResponseIds)
    {
        var hardware = await _hardwareControl.GetHardwareServices(updateHardwareResponseIds);
        if (hardware.Status == true)
        {
            return Ok(hardware);
        }
        return Ok(hardware);
    }
}