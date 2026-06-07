using Home_Security.Models.Enums;
namespace Home_Security.Models.DTOs;
public class CreatePersonDetailsDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
}
public class GetPersonDetailsDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public string ImageUrl { get; set; }
    public bool Disabled { get; set; }
    public List<GetAddressDto> GetAddressDtos { get; set; }
    public List<GetContactDetailsDto> GetContactDetailsDtos { get; set; }
}
public class UpdatePersonDetailsDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IFormFile ImageUrl { get; set; }
    // public List<GetContactDetailsDto> GetContactDetailsDtos { get; set; }
    // public List<UpdateAddressDto> UpdateAddressDtos { get; set; }
}