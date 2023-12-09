using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class PersonController : Controller
{
    IPersonControl _personControl;
    public PersonController(IPersonControl personControl)
    {
        _personControl = personControl;
    }
    [HttpPost("CreatePerson")]
    public async Task<IActionResult> CreatePerson([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] CreatePersonDto createPersonDto)
    {
        var person = await _personControl.CreatePerson(getAuthControlInfoDto, createPersonDto);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }

    [HttpPut("UpdatePerson")]
    public async Task<IActionResult> UpdatePerson([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdatePersonDto updatePersonDto)
    {
        var person = await _personControl.UpdatePerson(getAuthControlInfoDto, updatePersonDto);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }

    [HttpPut("AddPersonAddress")]
    public async Task<IActionResult> AddPersonAddress([FromQuery]GetAuthControlInfoDto getAuthControlInfoDto, int personId, [FromBody] List<CreateAddressDto> createAddressDtos)
    {
        var person = await _personControl.AddPersonAddress(getAuthControlInfoDto, personId, createAddressDtos);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }

    [HttpPut("AddPersonDetails")]
    public async Task<IActionResult> AddPersonDetails([FromQuery]GetAuthControlInfoDto getAuthControlInfoDto, int personId, [FromBody] List<CreateContactDetailsDto> createContactDetailsDtos)
    {
        var person = await _personControl.AddPersonDetails(getAuthControlInfoDto, personId, createContactDetailsDtos);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }

    [HttpGet("GetPersonById")]
    public async Task<IActionResult> GetPersonById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var person = await _personControl.GetPersonById(getAuthControlInfoDto, id);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }

    [HttpGet("GetAllPersons")]
    public async Task<IActionResult> GetAllPersons(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var person = await _personControl.GetAllPersons(getAuthControlInfoDto);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }

    [HttpPut("DeletePerson")]
    public async Task<IActionResult> DeletePerson(GetAuthControlInfoDto getAuthControlInfoDto, int personId)
    {
        var person = await _personControl.DeletePerson(getAuthControlInfoDto, personId);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }

    [HttpPut("DisablePerson")]
    public async Task<IActionResult> DisablePerson(GetAuthControlInfoDto getAuthControlInfoDto, int personId)
    {
        var person = await _personControl.DisablePerson(getAuthControlInfoDto, personId);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }

    [HttpPut("DeletePersonContactDetails")]
    public async Task<IActionResult> DeletePersonContactDetails(GetAuthControlInfoDto getAuthControlInfoDto, int personId, int personProfileDetailId)
    {
        var person = await _personControl.DeletePersonContactDetails(getAuthControlInfoDto, personId, personProfileDetailId);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }

    [HttpPut("DeletePersonContactAddress")]
    public async Task<IActionResult> DeletePersonContactAddress(GetAuthControlInfoDto getAuthControlInfoDto, int personId, int personProfileAddressId)
    {
        var person = await _personControl.DeletePersonContactAddress(getAuthControlInfoDto, personId, personProfileAddressId);
        if (person.Status == true)
        {
            return Ok(person);
        }
        return Ok(person);
    }
}
