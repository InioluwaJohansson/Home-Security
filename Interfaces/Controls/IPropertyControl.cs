using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface IPropertyControl
{
    public Task<BaseResponse> CreateProperty(GetAuthControlInfoDto getAuthControlInfoDto, CreatePropertyDto createPropertyDto);
    public Task<BaseResponse> UpdateProperty(GetAuthControlInfoDto getAuthControlInfoDto, UpdatePropertyDto updatePropertyDto);
    public Task<GetPropertyDto> GetPropertyById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<List<GetPropertyDto>> GetAllProperties(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteProperty(GetAuthControlInfoDto getAuthControlInfoDto, int propertyId);
}
