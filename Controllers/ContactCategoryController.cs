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
    public ContactCategoryController(IContactCategoryControl contactCategoryControl)
    {
        _contactCategoryControl = contactCategoryControl;
    }
    [Authorize]
    [HttpPost("CreateContactCategory")]
    public async Task<IActionResult> CreateContactCategory([FromBody] CreateContactCategoryDto createContactCategoryDto)
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
        };
        var contactCategory = await _contactCategoryControl.DeleteContactCategory(getAuthControlInfoDto, contactCategoryId);
        if (contactCategory.Status == true)
        {
            return Ok(contactCategory);
        }
        return Ok(contactCategory);
    }
}
