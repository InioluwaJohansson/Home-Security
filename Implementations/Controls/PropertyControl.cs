using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
namespace Home_Security.Implementations.Controls;
public class PropertyControl : IPropertyControl
{
    IAuthControl _authControl;
    IPropertyService _propertyService;
    ILogService _logService;
    public PropertyControl(IAuthControl authControl, IPropertyService propertyService, ILogService logService)
    {
        _authControl = authControl;
        _propertyService = propertyService;
        _logService = logService;
    }
    public async Task<BaseResponse> CreateProperty(GetAuthControlInfoDto getAuthControlInfoDto, CreatePropertyDto createPropertyDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var property = await _propertyService.CreateProperty(createPropertyDto, auth.Id);
            return property;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateProperty(GetAuthControlInfoDto getAuthControlInfoDto, UpdatePropertyDto updatePropertyDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var property = await _propertyService.UpdateProperty(updatePropertyDto, auth.Id);
            return property;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<GetPropertyDto> GetPropertyById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var property = await _propertyService.GetById(id);
            return property;
        }
        return new GetPropertyDto();
    }
    public async Task<List<GetPropertyDto>> GetAllProperties(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var property = await _propertyService.GetAllProperties();
            return property;
        }
        return new List<GetPropertyDto>();
    }
    public async Task<BaseResponse> DeleteProperty(GetAuthControlInfoDto getAuthControlInfoDto, int propertyId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var property = await _propertyService.DeleteProperty(propertyId, auth.Id);
            return property;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
}