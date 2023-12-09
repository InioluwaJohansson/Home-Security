using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class ContactCategoryController : Controller
{
    IContactCategoryControl _contactCategoryControl;
    public ContactCategoryController(IContactCategoryControl contactCategoryControl)
    {
        _contactCategoryControl = contactCategoryControl;
    }
    [HttpPost("CreateContactCategory")]
    public async Task<IActionResult> CreateContactCategory([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] CreateContactCategoryDto createContactCategoryDto)
    {
        var contactCategory = await _contactCategoryControl.CreateContactCategory(getAuthControlInfoDto, createContactCategoryDto);
        if (contactCategory.Status == true)
        {
            return Ok(contactCategory);
        }
        return Ok(contactCategory);
    }

    [HttpPut("UpdateContactCategory")]
    public async Task<IActionResult> UpdateContactCategory([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdateContactCategoryDto updateContactCategoryDto)
    {
        var contactCategory = await _contactCategoryControl.UpdateContactCategory(getAuthControlInfoDto, updateContactCategoryDto);
        if (contactCategory.Status == true)
        {
            return Ok(contactCategory);
        }
        return Ok(contactCategory);
    }

    [HttpGet("GetContactCategoryById")]
    public async Task<IActionResult> GetContactCategoryById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var contactCategory = await _contactCategoryControl.GetContactCategoryById(getAuthControlInfoDto, id);
        if (contactCategory.Status == true)
        {
            return Ok(contactCategory);
        }
        return Ok(contactCategory);
    }

    [HttpGet("GetAllContactCategories")]
    public async Task<IActionResult> GetAllContactCategories(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var contactCategory = await _contactCategoryControl.GetAllContactCategories(getAuthControlInfoDto);
        if (contactCategory.Status == true)
        {
            return Ok(contactCategory);
        }
        return Ok(contactCategory);
    }

    [HttpPut("DeleteContactCategory")]
    public async Task<IActionResult> DeleteContactCategory(GetAuthControlInfoDto getAuthControlInfoDto, int contactCategoryId)
    {
        var contactCategory = await _contactCategoryControl.DeleteContactCategory(getAuthControlInfoDto, contactCategoryId);
        if (contactCategory.Status == true)
        {
            return Ok(contactCategory);
        }
        return Ok(contactCategory);
    }
}
