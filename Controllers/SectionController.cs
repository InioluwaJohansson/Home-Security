using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class SectionController : Controller
{
    ISectionControl _sectionControl;
    public SectionController(ISectionControl sectionControl)
    {
        _sectionControl = sectionControl;
    }
    [Authorize]
    [HttpPost("CreateSection")]
    public async Task<IActionResult> CreateSection([FromQuery]CreateSectionDto createSectionDto)
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
        var section = await _sectionControl.CreateSection(getAuthControlInfoDto, createSectionDto);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }
    [Authorize]
    [HttpPut("UpdateSection")]
    public async Task<IActionResult> UpdateSection([FromQuery] UpdateSectionDto updateSectionDto)
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
        var section = await _sectionControl.UpdateSection(getAuthControlInfoDto, updateSectionDto);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }
    [Authorize]
    [HttpPost("GetSectionById")]
    public async Task<IActionResult> GetSectionById(int id)
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
        var section = await _sectionControl.GetSectionById(getAuthControlInfoDto, id);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }
    [Authorize]
    [HttpPost("GetSectionBySectionName")]
    public async Task<IActionResult> GetSectionBySectionName(string sectionName)
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
        var section = await _sectionControl.GetSectionBySectionName(getAuthControlInfoDto, sectionName);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }
    [Authorize]
    [HttpPost("GetAllSections")]
    public async Task<IActionResult> GetAllSections()
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
        var section = await _sectionControl.GetAllSections(getAuthControlInfoDto);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }
    [Authorize]
    [HttpPut("DeleteSection")]
    public async Task<IActionResult> DeleteSection(int sectionId)
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
        var section = await _sectionControl.DeleteSection(getAuthControlInfoDto, sectionId);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }
}
