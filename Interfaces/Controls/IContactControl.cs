using Home_Security.Entities.Identity;
using Home_Security.Implementations.Controls;
using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface IContactControl
{
    public Task<BaseResponse> CreateContact(GetAuthControlInfoDto getAuthControlInfoDto, CreateContactDto createContactDto);
    public Task<BaseResponse> UpdateContact(GetAuthControlInfoDto getAuthControlInfoDto, UpdateContactDto updateContactDto);
    public Task<BaseResponse> AddContactAddress(GetAuthControlInfoDto getAuthControlInfoDto, int contactId, List<CreateAddressDto> createAddressDtos);
    public Task<BaseResponse> AddContactDetails(GetAuthControlInfoDto getAuthControlInfoDto, int contactId, List<CreateContactDetailsDto> createContactDetailsDtos);
    public Task<ContactResponseModel> GetContactById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<ContactsResponseModel> GetContactByFirstName(GetAuthControlInfoDto getAuthControlInfoDto, string firstName);
    public Task<ContactsResponseModel> GetContactByLastName(GetAuthControlInfoDto getAuthControlInfoDto, string lastName);
    public Task<ContactsResponseModel> GetContactsByContactCategory(GetAuthControlInfoDto getAuthControlInfoDto, int contactCategory);
    public Task<ContactsResponseModel> GetAllContacts(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteContact(GetAuthControlInfoDto getAuthControlInfoDto, int contactId);
    public Task<BaseResponse> DeleteContactDetails(GetAuthControlInfoDto getAuthControlInfoDto, int contactId, int contactDetailId);
    public Task<BaseResponse> DeleteContactAddress(GetAuthControlInfoDto getAuthControlInfoDto, int contactId, int contactAddressId);
}
