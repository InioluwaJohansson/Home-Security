using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class SectionController : Controller
{
    ISectionControl _sectionControl;
    public SectionController(ISectionControl sectionControl)
    {
        _sectionControl = sectionControl;
    }
    [HttpPost("CreateSection")]
    public async Task<IActionResult> CreateSection([FromBody] GetAuthControlInfoDto getAuthControlInfoDto, [FromQuery]CreateSectionDto createSectionDto)
    {
        var section = await _sectionControl.CreateSection(getAuthControlInfoDto, createSectionDto);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }

    [HttpPut("UpdateSection")]
    public async Task<IActionResult> UpdateSection([FromBody] GetAuthControlInfoDto getAuthControlInfoDto, [FromQuery] UpdateSectionDto updateSectionDto)
    {
        var section = await _sectionControl.UpdateSection(getAuthControlInfoDto, updateSectionDto);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }

    [HttpGet("GetSectionById")]
    public async Task<IActionResult> GetSectionById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var section = await _sectionControl.GetSectionById(getAuthControlInfoDto, id);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }

    [HttpGet("GetSectionBySectionName")]
    public async Task<IActionResult> GetSectionBySectionName(GetAuthControlInfoDto getAuthControlInfoDto, string sectionName)
    {
        var section = await _sectionControl.GetSectionBySectionName(getAuthControlInfoDto, sectionName);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }

    [HttpGet("GetAllSections")]
    public async Task<IActionResult> GetAllSections(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var section = await _sectionControl.GetAllSections(getAuthControlInfoDto);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }

    [HttpPut("DeleteSection")]
    public async Task<IActionResult> DeleteSection(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var section = await _sectionControl.DeleteSection(getAuthControlInfoDto, sectionId);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }
}
