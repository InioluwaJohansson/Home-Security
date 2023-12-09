using Home_Security.Entities.Identity;
using Home_Security.Entities;
using Home_Security.Models.DTOs;
namespace Home_Security.Models.DTOs;
public class CreatePersonDto
{
    public string Relation { get; set; }
    public CreatePersonDetailsDto CreatePersonDetailsDto { get; set; }
    public CreateUserDto CreateUserDto { get; set; }
}
public class UpdatePersonDto
{
    public int Id { get; set; }
    public UpdatePersonDetailsDto UpdatePersonDetailsDto { get; set; }
    public UpdateUserDto UpdateUserDto { get; set; }
}
public class GetPersonDto
{
    public int Id { get; set; }
    public string PersonId { get; set; }
    public bool Disabled { get; set; }
    public GetPersonDetailsDto GetPersonDetailsDto { get; set; }
    public GetUserDto GetUserDto { get; set; }
}
public class PersonResponseModel : BaseResponse
{
    public GetPersonDto Data { get; set; }
}
public class PersonsResponseModel : BaseResponse
{
    public ICollection<GetPersonDto> Data { get; set; } = new HashSet<GetPersonDto>();
}