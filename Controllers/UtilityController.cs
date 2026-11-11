using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class UtilityController : Controller
{
    IUtilityControl _utilityControl;
    IJWTAuthentication _jwtAuthentication;
    public UtilityController(IUtilityControl utilityControl, IJWTAuthentication jwtAuthentication)
    {
        _utilityControl = utilityControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateUtility")]
    public async Task<IActionResult> CreateUtility([FromBody] CreateUtilityDto createUtilityDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var utility = await _utilityControl.CreateUtility(getAuthControlInfoDto, createUtilityDto);
        if (utility.Status == true)
        {
            return Ok(utility);
        }
        return Ok(utility);
    }
    [Authorize]
    [HttpPut("UpdateUtility")]
    public async Task<IActionResult> UpdateUtility([FromBody] UpdateUtilityDto updateUtilityDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var utility = await _utilityControl.UpdateUtility(getAuthControlInfoDto, updateUtilityDto);
        if (utility.Status == true)
        {
            return Ok(utility);
        }
        return Ok(utility);
    }
    [Authorize]
    [HttpPost("GetUtilityById")]
    public async Task<IActionResult> GetUtilityById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var utility = await _utilityControl.GetUtilityById(getAuthControlInfoDto, id);
        if (utility.Status == true)
        {
            return Ok(utility);
        }
        return Ok(utility);
    }
    [Authorize]
    [HttpPost("GetAllUtilitysBySectionId")]
    public async Task<IActionResult> GetAllUtilitysBySectionId(int sectionId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var utility = await _utilityControl.GetAllUtilitysBySectionId(getAuthControlInfoDto, sectionId);
        if (utility.Status == true)
        {
            return Ok(utility);
        }
        return Ok(utility);
    }
    [Authorize]
    [HttpPost("GetAllUtilitysByRoomId")]
    public async Task<IActionResult> GetAllUtilitysByRoomId(int roomId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var utility = await _utilityControl.GetAllUtilitysByRoomId(getAuthControlInfoDto, roomId);
        if (utility.Status == true)
        {
            return Ok(utility);
        }
        return Ok(utility);
    }
    [Authorize]
    [HttpPost("GetAllUtilitys")]
    public async Task<IActionResult> GetAllUtilitys()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var utility = await _utilityControl.GetAllUtilitys(getAuthControlInfoDto);
        if (utility.Status == true)
        {
            return Ok(utility);
        }
        return Ok(utility);
    }
    [Authorize]
    [HttpPut("DeleteUtility")]
    public async Task<IActionResult> DeleteUtility(int utilityId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var utility = await _utilityControl.DeleteUtility(getAuthControlInfoDto, utilityId);
        if (utility.Status == true)
        {
            return Ok(utility);
        }
        return Ok(utility);
    }
}
