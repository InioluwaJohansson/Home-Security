using Home_Security.Authentication;
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
    IJWTAuthentication _jwtAuthentication;
    public ContactController(IContactControl contactControl, IJWTAuthentication jwtAuthentication)
    {
        _contactControl = contactControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateContact")]
    public async Task<IActionResult> CreateContact([FromBody] CreateContactDto createContactDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var contact = await _contactControl.UpdateContact(getAuthControlInfoDto, updateContactDto);
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
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
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var contact = await _contactControl.DeleteContactAddress(getAuthControlInfoDto, contactId, contactAddressId);
        if (contact.Status == true)
        {
            return Ok(contact);
        }
        return Ok(contact);
    }
}
