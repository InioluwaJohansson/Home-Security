using Home_Security.Entities;
using Home_Security.Models.DTOs;
namespace Home_Security.Models.DTOs;
public class CreateContactDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IFormFile ImageUrl { get; set; }
    public int ContactCategory { get; set; }
    public int personId { get; set; }
    public List<CreateContactDetailsDto> ContactDetails { get; set; }
    public List<CreateAddressDto> Address { get; set; }
}
public class UpdateContactDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IFormFile ImageUrl { get; set; }
    public int ContactCategory { get; set; }
    public int personId { get; set; }
    public List<UpdateContactDetailsDto> ContactDetails { get; set; }
    public List<UpdateAddressDto> Address { get; set; }
}
public class GetContactDto : BaseDefaultDtoOther
{
    public int Id { get; set; }
    public string ContactId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ImageUrl { get; set; }
    public GetContactCategoryDto GetContactCategoryDto { get; set; }
    public List<GetContactDetailsDto> ContactDetails { get; set; }
    public List<GetAddressDto> Address { get; set; }
}
public class ContactResponseModel : BaseResponse
{
    public GetContactDto Data { get; set; }
}
public class ContactsResponseModel : BaseResponse
{
    public ICollection<GetContactDto> Data { get; set; } = new HashSet<GetContactDto>();
}