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
    public PersonController(IPersonControl personControl)
    {
        _personControl = personControl;
    }
    [Authorize]
    [HttpPost("CreatePerson")]
    public async Task<IActionResult> CreatePerson([FromBody] CreatePersonDto createPersonDto)
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
        var person = await _personControl.CreatePerson(getAuthControlInfoDto, createPersonDto);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPut("UpdatePerson")]
    public async Task<IActionResult> UpdatePerson([FromBody] UpdatePersonDto updatePersonDto)
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
        var person = await _personControl.UpdatePerson(getAuthControlInfoDto, updatePersonDto);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPut("AddPersonAddress")]
    public async Task<IActionResult> AddPersonAddress(int personId, [FromBody] List<CreateAddressDto> createAddressDtos)
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
        var person = await _personControl.AddPersonAddress(getAuthControlInfoDto, personId, createAddressDtos);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPut("AddPersonDetails")]
    public async Task<IActionResult> AddPersonDetails(int personId, [FromBody] List<CreateContactDetailsDto> createContactDetailsDtos)
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
        var person = await _personControl.AddPersonDetails(getAuthControlInfoDto, personId, createContactDetailsDtos);
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
    public async Task<IActionResult> DeletePersonContactDetails(int personId, int personProfileDetailId)
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
        var person = await _personControl.DeletePersonContactDetails(getAuthControlInfoDto, personId, personProfileDetailId);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
    [Authorize]
    [HttpPut("DeletePersonContactAddress")]
    public async Task<IActionResult> DeletePersonContactAddress(int personId, int personProfileAddressId)
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
        var person = await _personControl.DeletePersonContactAddress(getAuthControlInfoDto, personId, personProfileAddressId);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
}
