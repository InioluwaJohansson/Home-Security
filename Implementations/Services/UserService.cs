using BCrypt.Net;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Services;
public class UserService : IUserService
{
    IUserRepo _userRepo;
    public UserService(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }
    public async Task<UserLoginResponse> Login(string userName, string password)
    {
        var user = await _userRepo.GetUserByUserName(userName);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return new UserLoginResponse()
            {
                Data = new GetUserDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    RoleName = user.UserRole.Role.ToString(),
                    Role = user.UserRole.Role,
                    PersonId = user.PersonId
                },
                Message = "Login Successful",
                Status = true,
            };
        }
        return new UserLoginResponse()
        {
            Data = null,
            Message = "Invalid Username Or Password!",
            Status = false
        };
    }
    public async Task<BaseResponse> UpdateUser(UpdateUserDto updateUserDto)
    {
        var user = await _userRepo.GetUserById(updateUserDto.Id);
        if (user != null && BCrypt.Net.BCrypt.Verify(updateUserDto.Password, user.Password))
        {
            user.UserRole.Role = updateUserDto.Role;
            user.UserName = updateUserDto.UserName;
            user.LastModifiedOn = DateTime.Now;
            await _userRepo.Update(user);
            return new BaseResponse()
            {
                Message = "User Details Updated Successfully!",
                Status = false
            };
        }
        return new BaseResponse()
        {
            Message = "Error Updating User Details!",
            Status = false
        };
    }
    public async Task<BaseResponse> ChangePassword(UpdateUserPasswordDto updateUserPasswordDto)
    {
        var user = await _userRepo.GetUserByUserName(updateUserPasswordDto.UserName);
        if (user != null && BCrypt.Net.BCrypt.Verify(updateUserPasswordDto.AuthorizationCode, user.AuthorizationCode))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserPasswordDto.NewPassword);
            user.LastModifiedOn = DateTime.Now;
            await _userRepo.Update(user);
            return new BaseResponse()
            {
                Message = "Password Changed Successfully!",
                Status = true
            };
        }
        return new BaseResponse()
        {
            Message = "Error Updating Password!",
            Status = false
        };
    }
    public async Task<BaseResponse> ChangeAuthorizationCode(UpdateUserAuthorizationCode updateUserAuthorizationCode)
    {
        var user = await _userRepo.GetUserByUserName(updateUserAuthorizationCode.UserName);
        if (user != null && BCrypt.Net.BCrypt.Verify(updateUserAuthorizationCode.Password, user.Password))
        {
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(updateUserAuthorizationCode.NewAuthorizationCode);
            user.LastModifiedOn = DateTime.Now;
            await _userRepo.Update(user);
            return new BaseResponse()
            {
                Message = "Authorization Code Changed Successfully!",
                Status = true
            };
        }
        return new BaseResponse()
        {
            Message = "Error Updating Authorization Code!",
            Status = false
        };
    }
}