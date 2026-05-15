using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class ContactCategoryController : Controller
{
    IContactCategoryControl _contactCategoryControl;
    IJWTAuthentication _jwtAuthentication;
    public ContactCategoryController(IContactCategoryControl contactCategoryControl, IJWTAuthentication jwtAuthentication)
    {
        _contactCategoryControl = contactCategoryControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateContactCategory")]
    public async Task<IActionResult> CreateContactCategory([FromBody] CreateContactCategoryDto createContactCategoryDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var contactCategory = await _contactCategoryControl.CreateContactCategory(getAuthControlInfoDto, createContactCategoryDto);
        if (contactCategory.Status == true)
        {
            return Ok(contactCategory);
        }
        return Ok(contactCategory);
    }
    [Authorize]
    [HttpPut("UpdateContactCategory")]
    public async Task<IActionResult> UpdateContactCategory([FromBody] UpdateContactCategoryDto updateContactCategoryDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var contactCategory = await _contactCategoryControl.UpdateContactCategory(getAuthControlInfoDto, updateContactCategoryDto);
        if (contactCategory.Status == true)
        {
            return Ok(contactCategory);
        }
        return Ok(contactCategory);
    }
    [Authorize]
    [HttpPost("GetContactCategoryById")]
    public async Task<IActionResult> GetContactCategoryById(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var contactCategory = await _contactCategoryControl.GetContactCategoryById(getAuthControlInfoDto, id);
        if (contactCategory.Status == true)
        {
            return Ok(contactCategory);
        }
        return Ok(contactCategory);
    }
    [Authorize]
    [HttpPost("GetAllContactCategories")]
    public async Task<IActionResult> GetAllContactCategories()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var contactCategory = await _contactCategoryControl.GetAllContactCategories(getAuthControlInfoDto);
        if (contactCategory.Status == true)
        {
            return Ok(contactCategory);
        }
        return Ok(contactCategory);
    }
    [Authorize]
    [HttpPut("DeleteContactCategory")]
    public async Task<IActionResult> DeleteContactCategory(int contactCategoryId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var contactCategory = await _contactCategoryControl.DeleteContactCategory(getAuthControlInfoDto, contactCategoryId);
        if (contactCategory.Status == true)
        {
            return Ok(contactCategory);
        }
        return Ok(contactCategory);
    }
}
