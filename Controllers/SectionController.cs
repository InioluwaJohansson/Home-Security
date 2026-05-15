using Home_Security.Authentication;
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
    IJWTAuthentication _jwtAuthentication;
    public SectionController(ISectionControl sectionControl, IJWTAuthentication jwtAuthentication)
    {
        _sectionControl = sectionControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateSection")]
    public async Task<IActionResult> CreateSection([FromQuery]CreateSectionDto createSectionDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
       var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var section = await _sectionControl.DeleteSection(getAuthControlInfoDto, sectionId);
        if (section.Status == true)
        {
            return Ok(section);
        }
        return Ok(section);
    }
}
