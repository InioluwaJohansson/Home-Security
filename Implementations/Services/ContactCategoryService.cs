using Home_Security.Entities;
using Home_Security.Implementations.Repositories;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;

namespace Home_Security.Implementations.Services;
public class ContactCategoryService : IContactCategoryService
{
    IContactCategoryRepo _contactCategoryRepo;
    public ContactCategoryService(IContactCategoryRepo contactCategoryRepo)
    {
        _contactCategoryRepo = contactCategoryRepo;
    }
    public async Task<BaseResponse> CreateContactCategory(CreateContactCategoryDto createContactCategoryDto)
    {
        if (createContactCategoryDto != null)
        {
            var contactCategory = new ContactCategory()
            {
                CategoryId = $"CATEGORY{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                Name = createContactCategoryDto.Name,
                Description = createContactCategoryDto.Description,
                IsDeleted = false,
                PersonId = createContactCategoryDto.personId,
                CreatedBy = createContactCategoryDto.personId,
                LastModifiedBy = createContactCategoryDto.personId,
                LastModifiedOn = DateTime.Now,
                CreatedOn = DateTime.Now,
            };
            await _contactCategoryRepo.Create(contactCategory);
            return new BaseResponse()
            {
                Status = true,
                Message = "Contact Category Successfully Added!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Add Contact Category!"
        };
    }
    public async Task<BaseResponse> UpdateContactCategory(UpdateContactCategoryDto updateContactCategoryDto)
    {
        if (updateContactCategoryDto != null)
        {
            var contactCategory = await _contactCategoryRepo.Get(x => x.Id == updateContactCategoryDto.Id);
            contactCategory.Name = updateContactCategoryDto.Name ?? contactCategory.Name;
            contactCategory.Description = updateContactCategoryDto.Description ?? contactCategory.Description;
            contactCategory.LastModifiedBy = updateContactCategoryDto.personId;
            contactCategory.LastModifiedOn = DateTime.Now;
            await _contactCategoryRepo.Update(contactCategory);
            return new BaseResponse()
            {
                Status = true,
                Message = "Contact Category Updated Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update Contact Category!"
        };
    }
    public async Task<ContactCategoryResponseModel> GetById(int contactCategoryId)
    {
        var contactCategory = await _contactCategoryRepo.Get(x => x.Id == contactCategoryId && x.IsDeleted == false);
        if (contactCategory != null)
        {
            return new ContactCategoryResponseModel()
            {
                Data = GetDetails(contactCategory),
                Status = true,
                Message = "ContactCategory Retrieved Successfully!"
            };
        }
        return new ContactCategoryResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve ContactCategory!"
        };
    }
    public async Task<ContactCategoriesResponseModel> GetAllContactCategories()
    {
        var contactCategorys = await _contactCategoryRepo.GetByExpression(x => x.IsDeleted == false);
        if (contactCategorys != null)
        {
            return new ContactCategoriesResponseModel()
            {
                Data = contactCategorys.Select(x => GetDetails(x)).ToList(),
                Message = "Contact Categories Retrieved Successfully!",
                Status = true
            };
        }
        return new ContactCategoriesResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Contact Categories!",
            Status = false
        };
    }
    public async Task<BaseResponse> Delete(int contactCategoryId, int personId)
    {
        var contactCategory = await _contactCategoryRepo.Get(x => x.Id == contactCategoryId);
        if (contactCategory != null)
        {
            contactCategory.LastModifiedBy = personId;
            contactCategory.LastModifiedOn = DateTime.Now;
            contactCategory.DeletedOn = DateTime.Now;
            contactCategory.DeletedBy = personId;
            contactCategory.IsDeleted = true;
            await _contactCategoryRepo.Update(contactCategory);
            return new BaseResponse()
            {
                Status = true,
                Message = "Contact Category Deleted Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Contact Category!"
        };
    }
    public GetContactCategoryDto GetDetails(ContactCategory contactCategory)
    {
        return new GetContactCategoryDto()
        {
            Id = contactCategory.Id,
            Name = contactCategory.Name,
            CategoryId = contactCategory.CategoryId,
            personId = contactCategory.PersonId,
            LastModifiedBy = contactCategory.LastModifiedBy,
            DeletedBy = contactCategory.DeletedBy,
            CreatedBy = contactCategory.CreatedBy,
            LastModifiedOn = contactCategory.LastModifiedOn,
            CreatedOn = contactCategory.CreatedOn,
            IsDeleted = contactCategory.IsDeleted,
        };
    }
}