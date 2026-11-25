using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class PropertyController : Controller
{
    IPropertyControl _propertyControl;
    IJWTAuthentication _jwtAuthentication;
    public PropertyController(IPropertyControl propertyControl, IJWTAuthentication jwtAuthentication)
    {
        _propertyControl = propertyControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateProperty")]
    public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyDto createPropertyDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request);
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var property = await _propertyControl.CreateProperty(getAuthControlInfoDto, createPropertyDto);
        if (property.Status == true)
        {
            return Ok(property);
        }
        return Ok(property);
    }
    [Authorize]
    [HttpPut("UpdateProperty")]
    public async Task<IActionResult> UpdateProperty([FromBody] UpdatePropertyDto updatePropertyDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request);
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var property = await _propertyControl.UpdateProperty(getAuthControlInfoDto, updatePropertyDto);
        if (property.Status == true)
        {
            return Ok(property);
        }
        return Ok(property);
    }
    [Authorize]
    [HttpPost("GetPropertyById")]
    public async Task<IActionResult> GetPropertyById(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request);
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var property = await _propertyControl.GetPropertyById(getAuthControlInfoDto, id);
        return Ok(property);
    }
    [Authorize]
    [HttpPost("GetAllProperties")]
    public async Task<IActionResult> GetAllProperties()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request);
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var property = await _propertyControl.GetAllProperties(getAuthControlInfoDto);
        return Ok(property);
    }
    [Authorize]
    [HttpPut("DeleteProperty")]
    public async Task<IActionResult> DeleteProperty(int propertyId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request);
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var property = await _propertyControl.DeleteProperty(getAuthControlInfoDto, propertyId);
        if (property.Status == true)
        {
            return Ok(property);
        }
        return Ok(property);
    }
}
