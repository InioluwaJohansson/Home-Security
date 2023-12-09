using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class ContactController : Controller
{
    IContactControl _contactControl;
    public ContactController(IContactControl contactControl)
    {
        _contactControl = contactControl;
    }
    [HttpPost("CreateContact")]
    public async Task<IActionResult> CreateContact([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] CreateContactDto createContactDto)
    {
        var contact = await _contactControl.CreateContact(getAuthControlInfoDto, createContactDto);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }

    [HttpPut("UpdateContact")]
    public async Task<IActionResult> UpdateContact([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdateContactDto updateContactDto)
    {
        var contact = await _contactControl.UpdateContact(getAuthControlInfoDto, updateContactDto);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }

    [HttpPut("AddContactAddress")]
    public async Task<IActionResult> AddContactAddress(GetAuthControlInfoDto getAuthControlInfoDto, int contactId, [FromForm] List<CreateAddressDto> createAddressDtos)
    {
        var contact = await _contactControl.AddContactAddress(getAuthControlInfoDto, contactId, createAddressDtos);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }

    [HttpPut("AddContactDetails")]
    public async Task<IActionResult> AddContactDetails(GetAuthControlInfoDto getAuthControlInfoDto, int contactId, [FromForm] List<CreateContactDetailsDto> createContactDetailsDtos)
    {
        var contact = await _contactControl.AddContactDetails(getAuthControlInfoDto, contactId, createContactDetailsDtos);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }

    [HttpGet("GetContactById")]
    public async Task<IActionResult> GetContactById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var contact = await _contactControl.GetContactById(getAuthControlInfoDto, id);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }

    [HttpGet("GetContactByFirstName")]
    public async Task<IActionResult> GetContactByFirstName(GetAuthControlInfoDto getAuthControlInfoDto, string firstName)
    {
        var contact = await _contactControl.GetContactByFirstName(getAuthControlInfoDto, firstName);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }

    [HttpGet("GetContactByLastName")]
    public async Task<IActionResult> GetContactByLastName(GetAuthControlInfoDto getAuthControlInfoDto, string lastName)
    {
        var contact = await _contactControl.GetContactByLastName(getAuthControlInfoDto, lastName);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }

    [HttpGet("GetContactsByContactCategory")]
    public async Task<IActionResult> GetContactsByContactCategory(GetAuthControlInfoDto getAuthControlInfoDto, int contactCategory)
    {
        var contact = await _contactControl.GetContactsByContactCategory(getAuthControlInfoDto, contactCategory);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }

    [HttpGet("GetAllContacts")]
    public async Task<IActionResult> GetAllContacts(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var contact = await _contactControl.GetAllContacts(getAuthControlInfoDto);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }

    [HttpPut("DeleteContact")]
    public async Task<IActionResult> DeleteContact(GetAuthControlInfoDto getAuthControlInfoDto, int contactId)
    {
        var contact = await _contactControl.DeleteContact(getAuthControlInfoDto, contactId);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }

    [HttpPut("DeleteContactDetails")]
    public async Task<IActionResult> DeleteContactDetails(GetAuthControlInfoDto getAuthControlInfoDto, int contactId, int contactDetailId)
    {
        var contact = await _contactControl.DeleteContactDetails(getAuthControlInfoDto, contactId, contactDetailId);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }

    [HttpPut("DeleteContactAddress")]
    public async Task<IActionResult> DeleteContactAddress(GetAuthControlInfoDto getAuthControlInfoDto, int contactId, int contactAddressId)
    {
        var contact = await _contactControl.DeleteContactAddress(getAuthControlInfoDto, contactId, contactAddressId);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
}
