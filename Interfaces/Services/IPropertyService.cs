using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
namespace Home_Security.Interfaces.Services;
public interface IPropertyService
{
    public Task<BaseResponse> CreateProperty(CreatePropertyDto createPropertyDto, int personId);
    public Task<BaseResponse> UpdateProperty(UpdatePropertyDto updatePropertyDto, int personId);
    public Task<GetPropertyDto> GetById(int utilityId);
    public Task<List<GetPropertyDto>> GetAllProperties();
    public Task<BaseResponse> DeleteProperty(int propertyId, int personId);
}