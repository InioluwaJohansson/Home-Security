using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Services;
public interface IUserService
{
    public Task<UserLoginResponse> Login(string userName, string password);
    public Task<BaseResponse> UpdateUser(UpdateUserDto updateUserDto);
    public Task<BaseResponse> ChangePassword(UpdateUserPasswordDto updateUserPasswordDto);
    public Task<BaseResponse> ChangeAuthorizationCode(UpdateUserAuthorizationCode updateUserAuthorizationCode);
}
