using Home_Security.Entities.Identity;
using Home_Security.Implementations.Controls;
using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface IPersonControl
{
    public Task<BaseResponse> CreatePerson(GetAuthControlInfoDto getAuthControlInfoDto, CreatePersonDto createPersonDto);
    public Task<BaseResponse> UpdatePerson(GetAuthControlInfoDto getAuthControlInfoDto, UpdatePersonDto updatePersonDto);
    public Task<BaseResponse> AddPersonAddress(GetAuthControlInfoDto getAuthControlInfoDto, int personId, List<CreateAddressDto> createAddressDtos);
    public Task<BaseResponse> AddPersonDetails(GetAuthControlInfoDto getAuthControlInfoDto, int personId, List<CreateContactDetailsDto> createContactDetailsDtos);
    public Task<PersonResponseModel> GetPersonById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<PersonsResponseModel> GetAllPersons(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeletePerson(GetAuthControlInfoDto getAuthControlInfoDto, int personId);
    public Task<BaseResponse> DisablePerson(GetAuthControlInfoDto getAuthControlInfoDto, int personId);
    public Task<BaseResponse> DeletePersonContactDetails(GetAuthControlInfoDto getAuthControlInfoDto, int personId, int personDetailId);
    public Task<BaseResponse> DeletePersonContactAddress(GetAuthControlInfoDto getAuthControlInfoDto, int personId, int personAddressId);
}
