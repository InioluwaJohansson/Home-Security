using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface IPersonService
{
    public Task<BaseResponse> CreatePerson(CreatePersonDto createPersonDto);
    public Task<BaseResponse> UpdatePerson(UpdatePersonDto updatePersonDto);
    public Task<BaseResponse> AddPersonAddress(int personId, List<CreateAddressDto> createAddressDtos);
    public Task<BaseResponse> AddPersonContactDetails(int personId, List<CreateContactDetailsDto> createContactDetailsDto);
    public Task<PersonResponseModel> GetPersonById(int id);
    public Task<PersonsResponseModel> GetAllPersons();
    public Task<BaseResponse> DisablePerson(int id);
    public Task<BaseResponse> Delete(int id, int personId);
    public Task<BaseResponse> DeleteContactDetails(int id, int personId);
    public Task<BaseResponse> DeleteContactAddress(int id, int personId);
}