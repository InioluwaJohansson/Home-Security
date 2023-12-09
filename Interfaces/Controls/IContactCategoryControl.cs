using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Controls;
public interface IContactCategoryControl
{
    public Task<BaseResponse> CreateContactCategory(GetAuthControlInfoDto getAuthControlInfoDto, CreateContactCategoryDto createContactCategoryDto);
    public Task<BaseResponse> UpdateContactCategory(GetAuthControlInfoDto getAuthControlInfoDto, UpdateContactCategoryDto updateContactCategoryDto);
    public Task<ContactCategoryResponseModel> GetContactCategoryById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<ContactCategoriesResponseModel> GetAllContactCategories(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteContactCategory(GetAuthControlInfoDto getAuthControlInfoDto, int cameraId);
}
