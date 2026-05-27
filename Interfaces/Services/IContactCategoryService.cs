using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Services;
public interface IContactCategoryService
{
    public Task<BaseResponse> CreateContactCategory(CreateContactCategoryDto createContactCategoryDto, int personId);
    public Task<BaseResponse> UpdateContactCategory(UpdateContactCategoryDto updateContactCategoryDto, int personId);
    public Task<ContactCategoryResponseModel> GetById(int contactCategoryId);
    public Task<ContactCategoriesResponseModel> GetAllContactCategories();
    public Task<BaseResponse> Delete(int contactCategoryId, int personId);
}