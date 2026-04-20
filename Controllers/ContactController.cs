using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class ContactController : Controller
{
    IContactControl _contactControl;
    public ContactController(IContactControl contactControl)
    {
        _contactControl = contactControl;
    }
    [Authorize]
    [HttpPost("CreateContact")]
    public async Task<IActionResult> CreateContact([FromBody] CreateContactDto createContactDto)
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
        var contact = await _contactControl.CreateContact(getAuthControlInfoDto, createContactDto);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
    [Authorize]
    [HttpPut("UpdateContact")]
    public async Task<IActionResult> UpdateContact([FromBody] UpdateContactDto updateContactDto)
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
        var contact = await _contactControl.UpdateContact(getAuthControlInfoDto, updateContactDto);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
    [Authorize]
    [HttpPut("AddContactAddress")]
    public async Task<IActionResult> AddContactAddress(int contactId, [FromForm] List<CreateAddressDto> createAddressDtos)
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
        var contact = await _contactControl.AddContactAddress(getAuthControlInfoDto, contactId, createAddressDtos);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
    [Authorize]
    [HttpPut("AddContactDetails")]
    public async Task<IActionResult> AddContactDetails(int contactId, [FromForm] List<CreateContactDetailsDto> createContactDetailsDtos)
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
        var contact = await _contactControl.AddContactDetails(getAuthControlInfoDto, contactId, createContactDetailsDtos);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
    [Authorize]
    [HttpPost("GetContactById")]
    public async Task<IActionResult> GetContactById(int id)
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
        var contact = await _contactControl.GetContactById(getAuthControlInfoDto, id);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
    [Authorize]
    [HttpPost("GetContactByFirstName")]
    public async Task<IActionResult> GetContactByFirstName(string firstName)
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
        var contact = await _contactControl.GetContactByFirstName(getAuthControlInfoDto, firstName);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
    [Authorize]
    [HttpPost("GetContactByLastName")]
    public async Task<IActionResult> GetContactByLastName(string lastName)
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
        var contact = await _contactControl.GetContactByLastName(getAuthControlInfoDto, lastName);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
    [Authorize]
    [HttpPost("GetContactsByContactCategory")]
    public async Task<IActionResult> GetContactsByContactCategory(int contactCategory)
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
        var contact = await _contactControl.GetContactsByContactCategory(getAuthControlInfoDto, contactCategory);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
    [Authorize]
    [HttpPost("GetAllContacts")]
    public async Task<IActionResult> GetAllContacts()
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
        var contact = await _contactControl.GetAllContacts(getAuthControlInfoDto);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
    [Authorize]
    [HttpPut("DeleteContact")]
    public async Task<IActionResult> DeleteContact(int contactId)
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
        var contact = await _contactControl.DeleteContact(getAuthControlInfoDto, contactId);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
    [Authorize]
    [HttpPut("DeleteContactDetails")]
    public async Task<IActionResult> DeleteContactDetails(int contactId, int contactDetailId)
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
        var contact = await _contactControl.DeleteContactDetails(getAuthControlInfoDto, contactId, contactDetailId);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
    [Authorize]
    [HttpPut("DeleteContactAddress")]
    public async Task<IActionResult> DeleteContactAddress(int contactId, int contactAddressId)
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
        var contact = await _contactControl.DeleteContactAddress(getAuthControlInfoDto, contactId, contactAddressId);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
}
