using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface IContactService
{
    public Task<BaseResponse> CreateContact(CreateContactDto createContactDto);
    public Task<BaseResponse> UpdateContact(UpdateContactDto updateContactDto);
    public Task<BaseResponse> AddContactAddress(int contactId, int personId, List<CreateAddressDto> createAddressDto);
    public Task<BaseResponse> AddContactDetails(int contactId, int personId, List<CreateContactDetailsDto> createContactDetailsDto);
    public Task<ContactResponseModel> GetContactById(int id);
    public Task<ContactsResponseModel> GetContactByFirstName(string firstName);
    public Task<ContactsResponseModel> GetContactByLastName(string lastName);
    public Task<ContactsResponseModel> GetContactByContactCategory(int id);
    public Task<ContactsResponseModel> GetAllContacts();
    public Task<BaseResponse> Delete(int id, int personId);
    public Task<BaseResponse> DeleteContactDetails(int id, int personId);
    public Task<BaseResponse> DeleteContactAddress(int id, int personId);
}