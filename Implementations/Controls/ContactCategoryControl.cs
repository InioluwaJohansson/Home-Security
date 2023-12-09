using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Implementations.Services;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Controls.Defaults;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Controls;
public class ContactCategoryControl : IContactCategoryControl
{
    IAuthControl _authControl;
    IContactCategoryService _contactCategoryService;
    ILogService _logService;
    IObjectDefault _objectDefault;
    public ContactCategoryControl(IAuthControl authControl, IContactCategoryService contactCategoryService, ILogService logService, IObjectDefault objectDefault)
    {
        _authControl = authControl;
        _contactCategoryService = contactCategoryService;
        _logService = logService;
        _objectDefault = objectDefault;
    }
    public async Task<BaseResponse> CreateContactCategory(GetAuthControlInfoDto getAuthControlInfoDto, CreateContactCategoryDto createContactCategoryDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            createContactCategoryDto.personId = auth.Id;
            var contactCategory = await _contactCategoryService.CreateContactCategory(createContactCategoryDto);
            if (contactCategory.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "A Contact Category was newly added!";
                await _logService.CreateLog(log);
            }
            return contactCategory;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateContactCategory(GetAuthControlInfoDto getAuthControlInfoDto, UpdateContactCategoryDto updateContactCategoryDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            updateContactCategoryDto.personId = auth.Id;
            var contactCategory = await _contactCategoryService.UpdateContactCategory(updateContactCategoryDto);
            if (contactCategory.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateContactCategoryDto.Name} Contact Category was recently updated!";
                await _logService.CreateLog(log);
            }
            return contactCategory;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<ContactCategoryResponseModel> GetContactCategoryById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var contactCategory = await _contactCategoryService.GetById(id);
            return contactCategory;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ContactCategoryResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action!"
            };
        }
        return new ContactCategoryResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<ContactCategoriesResponseModel> GetAllContactCategories(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var contactCategory = await _contactCategoryService.GetAllContactCategories();
            return contactCategory;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ContactCategoriesResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action!"
            };
        }
        return new ContactCategoriesResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<BaseResponse> DeleteContactCategory(GetAuthControlInfoDto getAuthControlInfoDto, int contactCategoryId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var contactCategory = await _contactCategoryService.Delete(contactCategoryId, auth.Id);
            if (contactCategory.Status == true)
            {
                var getContactCategory = await _objectDefault.ContactCategoryName(contactCategoryId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getContactCategory} Contact Category was Deleted!";
                await _logService.CreateLog(log);
            }
            return contactCategory;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action!";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
}