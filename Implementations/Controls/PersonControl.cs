using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Controls.Defaults;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;

namespace Home_Security.Implementations.Controls;
public class PersonControl : IPersonControl
{
    IAuthControl _authControl;
    IPersonService _personService;
    ILogService _logService;
    IObjectDefault _objectDefault;
    public PersonControl(IAuthControl authControl, IPersonService personService, ILogService logService, IObjectDefault objectDefault)
    {
        _authControl = authControl;
        _personService = personService;
        _logService = logService;
        _objectDefault = objectDefault;
    }
    public async Task<BaseResponse> CreatePerson(GetAuthControlInfoDto getAuthControlInfoDto, CreatePersonDto createPersonDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var person = await _personService.CreatePerson(createPersonDto);
            if (person.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "A Person Profile was newly added!";
                await _logService.CreateLog(log);
            }
            return person;
        }
        else if (createPersonDto.CreateUserDto.Role == Role.Owner)
        {
            var person = await _personService.CreatePerson(createPersonDto);
            if (person.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "A Person Profile was newly added!";
                await _logService.CreateLog(log);
            }
            return person;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdatePerson(GetAuthControlInfoDto getAuthControlInfoDto, UpdatePersonDto updatePersonDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var person = await _personService.UpdatePerson(updatePersonDto);
            if (person.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updatePersonDto.UpdatePersonDetailsDto.FirstName} {updatePersonDto.UpdatePersonDetailsDto.LastName} Profile was recently updated!";
                await _logService.CreateLog(log);
            }
            return person;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> AddPersonAddress(GetAuthControlInfoDto getAuthControlInfoDto, int personId, List<CreateAddressDto> createAddressDtos)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var person = await _personService.AddPersonAddress(personId, createAddressDtos);
            if (person.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                var getPerson = await _objectDefault.PersonName(personId);
                log.ActionType = "Update";
                log.LogDetails = $"{getPerson} Profile Address was recently updated!";
                await _logService.CreateLog(log);
            }
            return person;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> AddPersonDetails(GetAuthControlInfoDto getAuthControlInfoDto, int personId, List<CreateContactDetailsDto> createContactDetailsDtos)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var person = await _personService.AddPersonContactDetails(personId, createContactDetailsDtos);
            if (person.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                var getPerson = await _objectDefault.PersonName(personId);
                log.ActionType = "Update";
                log.LogDetails = $"{getPerson} Profile Details was recently updated!";
                await _logService.CreateLog(log);
            }
            return person;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<PersonResponseModel> GetPersonById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var person = await _personService.GetPersonById(id);
            return person;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new PersonResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action!"
            };
        }
        return new PersonResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<PersonsResponseModel> GetAllPersons(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var person = await _personService.GetAllPersons();
            return person;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new PersonsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action!"
            };
        }
        return new PersonsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<BaseResponse> DeletePerson(GetAuthControlInfoDto getAuthControlInfoDto, int personId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var person = await _personService.Delete(personId, auth.Id);
            if (person.Status == true)
            {
                var getPerson = await _objectDefault.PersonName(personId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getPerson} Profile was Deleted!";
                await _logService.CreateLog(log);
            }
            return person;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action!";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> DisablePerson(GetAuthControlInfoDto getAuthControlInfoDto, int personId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var person = await _personService.DisablePerson(personId);
            if (person.Status == true)
            {
                var getPerson = await _objectDefault.PersonName(personId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Disable";
                log.LogDetails = $"{getPerson} Profile was Disabled!";
                await _logService.CreateLog(log);
            }
            return person;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action!";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> DeletePersonContactDetails(GetAuthControlInfoDto getAuthControlInfoDto, int personId, int personDetailId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var person = await _personService.DeleteContactDetails(personDetailId, auth.Id);
            if (person.Status == true)
            {
                var getPerson = await _objectDefault.PersonName(personId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getPerson} Person Detail was Deleted!";
                await _logService.CreateLog(log);
            }
            return person;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action!";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> DeletePersonContactAddress(GetAuthControlInfoDto getAuthControlInfoDto, int personId, int personAddressId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var person = await _personService.DeleteContactAddress(personAddressId, auth.Id);
            if (person.Status == true)
            {
                var getPerson = await _objectDefault.PersonName(personId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getPerson} Person Address was Deleted!";
                await _logService.CreateLog(log);
            }
            return person;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action!";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
}
