using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class PersonController : Controller
{
    IPersonControl _personControl;
    IJWTAuthentication _jwtAuthentication;
    public PersonController(IPersonControl personControl, IJWTAuthentication jwtAuthentication)
    {
        _personControl = personControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreatePerson")]
    public async Task<IActionResult> CreatePerson([FromBody] CreatePersonDto createPersonDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var person = await _personControl.CreatePerson(getAuthControlInfoDto, createPersonDto);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPut("UpdatePerson")]
    public async Task<IActionResult> UpdatePerson(UpdatePersonDto updatePersonDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var person = await _personControl.UpdatePerson(getAuthControlInfoDto, updatePersonDto);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPut("AddPersonAddress")]
    public async Task<IActionResult> AddPersonAddress([FromBody] List<CreateAddressDto> createAddressDtos)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var person = await _personControl.AddPersonAddress(getAuthControlInfoDto, createAddressDtos);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPut("AddPersonDetails")]
    public async Task<IActionResult> AddPersonDetails([FromBody] List<CreateContactDetailsDto> createContactDetailsDtos)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var person = await _personControl.AddPersonDetails(getAuthControlInfoDto, createContactDetailsDtos);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPost("GetPersonById")]
    public async Task<IActionResult> GetPersonById(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var person = await _personControl.GetPersonById(getAuthControlInfoDto, id);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPost("GetAllPersons")]
    public async Task<IActionResult> GetAllPersons()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var person = await _personControl.GetAllPersons(getAuthControlInfoDto);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPut("DeletePerson")]
    public async Task<IActionResult> DeletePerson(int personId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var person = await _personControl.DeletePerson(getAuthControlInfoDto, personId);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPut("DisablePerson")]
    public async Task<IActionResult> DisablePerson(int personId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var person = await _personControl.DisablePerson(getAuthControlInfoDto, personId);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPut("DeletePersonContactDetails")]
    public async Task<IActionResult> DeletePersonContactDetails(int personProfileDetailId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var person = await _personControl.DeletePersonContactDetails(getAuthControlInfoDto, personProfileDetailId);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPut("DeletePersonContactAddress")]
    public async Task<IActionResult> DeletePersonContactAddress(int personProfileAddressId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var person = await _personControl.DeletePersonContactAddress(getAuthControlInfoDto, personProfileAddressId);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
}
