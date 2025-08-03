using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class LogController : Controller
{
    ILogControl _logControl;
    public LogController(ILogControl logControl)
    {
        _logControl = logControl;
    }

    [HttpPost("GetLogById")]
    public async Task<IActionResult> GetLogById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var log = await _logControl.GetLogById(getAuthControlInfoDto, id);
        if (log.Status == true)
        {
            return Ok(log);
        }
        return Ok(log);
    }

    [HttpPost("GetAllLogsByPersonId")]
    public async Task<IActionResult> GetAllLogsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int personId)
    {
        var log = await _logControl.GetAllLogsByPersonId(getAuthControlInfoDto, personId);
        if (log.Status == true)
        {
            return Ok(log);
        }
        return Ok(log);
    }

    [HttpPost("GetAllLogsByActionType")]
    public async Task<IActionResult> GetAllLogsByActionType(GetAuthControlInfoDto getAuthControlInfoDto, string actionType)
    {
        var log = await _logControl.GetAllLogsByActionType(getAuthControlInfoDto, actionType);
        if (log.Status == true)
        {
            return Ok(log);
        }
        return Ok(log);
    }

    [HttpPost("GetAllLogs")]
    public async Task<IActionResult> GetAllLogs(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var log = await _logControl.GetAllLogs(getAuthControlInfoDto);
        if (log.Status == true)
        {
            return Ok(log);
        }
        return Ok(log);
    }
}
