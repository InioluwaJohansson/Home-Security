using Home_Security.Entities.Identity;
namespace Home_Security.Models.DTOs;
public class CreateUserDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string AuthorizationCode { get; set; }
    public Role Role { get; set; }
}
public class GetUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string RoleName { get; set; }
    public Role Role { get; set; }
    public int PersonId { get; set; }
    public string AuthorizationCode { get; set; }
}
public class UpdateUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public Role Role { get; set; }
}
public class UpdateUserPasswordDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string NewPassword { get; set; }
    public string TokenCode { get; set; }
    public string AuthorizationCode { get; set; }
}
public class UpdateUserAuthorizationCode
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string TokenCode { get; set; }
    public string NewAuthorizationCode { get; set; }
}
public class NFIDLoginDto
{
    public string Token { get; set; }
    public string NFIDCode { get; set; }
}
public class UserLoginResponse : BaseResponse
{
    public GetUserDto Data { get; set; }
    public string Token { get; set; }
    public string ImageUrl { get; set; }
}
