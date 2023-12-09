using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Implementations.Services;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Controls.Defaults;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Controls;
public class ContactControl : IContactControl
{
    IAuthControl _authControl;
    IContactService _contactService;
    ILogService _logService;
    IObjectDefault _objectDefault;
    public ContactControl(IAuthControl authControl, IContactService contactService, ILogService logService, IObjectDefault objectDefault)
    {
        _authControl = authControl;
        _contactService = contactService;
        _logService = logService;
        _objectDefault = objectDefault;
    }
    public async Task<BaseResponse> CreateContact(GetAuthControlInfoDto getAuthControlInfoDto, CreateContactDto createContactDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            createContactDto.personId = auth.Id;
            var contact = await _contactService.CreateContact(createContactDto);
            if (contact.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "A Contact Category was newly added!";
                await _logService.CreateLog(log);
            }
            return contact;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateContact(GetAuthControlInfoDto getAuthControlInfoDto, UpdateContactDto updateContactDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            updateContactDto.personId = auth.Id;
            var contact = await _contactService.UpdateContact(updateContactDto);
            if (contact.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateContactDto.FirstName} {updateContactDto.LastName} Contact was recently updated!";
                await _logService.CreateLog(log);
            }
            return contact;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> AddContactAddress(GetAuthControlInfoDto getAuthControlInfoDto, int contactId, List<CreateAddressDto> createAddressDtos)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var contact = await _contactService.AddContactAddress(contactId, auth.Id, createAddressDtos);
            if (contact.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                var getContact = await _objectDefault.ContactName(contactId);
                log.ActionType = "Update";
                log.LogDetails = $"{getContact} Contact Address was recently updated!";
                await _logService.CreateLog(log);
            }
            return contact;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> AddContactDetails(GetAuthControlInfoDto getAuthControlInfoDto, int contactId, List<CreateContactDetailsDto> createContactDetailsDtos)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var contact = await _contactService.AddContactDetails(contactId, auth.Id, createContactDetailsDtos);
            if (contact.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                var getContact = await _objectDefault.ContactName(contactId);
                log.ActionType = "Update";
                log.LogDetails = $"{getContact} Contact Details was recently updated!";
                await _logService.CreateLog(log);
            }
            return contact;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<ContactResponseModel> GetContactById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var contact = await _contactService.GetContactById(id);
            return contact;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ContactResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action!"
            };
        }
        return new ContactResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<ContactsResponseModel> GetContactByFirstName(GetAuthControlInfoDto getAuthControlInfoDto, string firstName)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var contact = await _contactService.GetContactByFirstName(firstName);
            return contact;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ContactsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action!"
            };
        }
        return new ContactsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<ContactsResponseModel> GetContactByLastName(GetAuthControlInfoDto getAuthControlInfoDto, string lastName)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var contact = await _contactService.GetContactByLastName(lastName);
            return contact;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ContactsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action!"
            };
        }
        return new ContactsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<ContactsResponseModel> GetContactsByContactCategory(GetAuthControlInfoDto getAuthControlInfoDto, int contactCategory)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var contact = await _contactService.GetContactByContactCategory(contactCategory);
            return contact;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ContactsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action!"
            };
        }
        return new ContactsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<ContactsResponseModel> GetAllContacts(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var contact = await _contactService.GetAllContacts();
            return contact;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ContactsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action!"
            };
        }
        return new ContactsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<BaseResponse> DeleteContact(GetAuthControlInfoDto getAuthControlInfoDto, int contactId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var contact = await _contactService.Delete(contactId, auth.Id);
            if (contact.Status == true)
            {
                var getContact = await _objectDefault.ContactName(contactId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getContact} Contact was Deleted!";
                await _logService.CreateLog(log);
            }
            return contact;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action!";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> DeleteContactDetails(GetAuthControlInfoDto getAuthControlInfoDto, int contactId, int contactDetailId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var contact = await _contactService.DeleteContactDetails(contactDetailId, auth.Id);
            if (contact.Status == true)
            {
                var getContact = await _objectDefault.ContactName(contactId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getContact} Contact Detail was Deleted!";
                await _logService.CreateLog(log);
            }
            return contact;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action!";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> DeleteContactAddress(GetAuthControlInfoDto getAuthControlInfoDto, int contactId, int contactAddressId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var contact = await _contactService.DeleteContactAddress(contactAddressId, auth.Id);
            if (contact.Status == true)
            {
                var getContact = await _objectDefault.ContactName(contactId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getContact} Contact Address was Deleted!";
                await _logService.CreateLog(log);
            }
            return contact;
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