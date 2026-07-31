using BCrypt.Net;
using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Services;
public class UserService : IUserService
{
    IUserRepo _userRepo;
    IPersonRepo _personRepo;
    IJWTAuthentication _jWTAuthentication;
    ILogService _logService;
    IAuthControl _authControl;
    public UserService(IUserRepo userRepo, IPersonRepo personRepo, IJWTAuthentication jWTAuthentication, ILogService logService, IAuthControl authControl)
    {
        _userRepo = userRepo;
        _personRepo = personRepo;
        _jWTAuthentication = jWTAuthentication;
        _logService = logService;
        _authControl = authControl;
    }
    public async Task<UserLoginResponse> Login(string userName, string password)
    {
        var user = await _userRepo.GetUserByUserName(userName);
        var log = _authControl.CreateLog(user.Id);
        log.ActionType = "Attempted Login";
        log.LogDetails = $"{user.UserName} attempted to login!";
        await _logService.CreateLog(log);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            var data = new UserLoginResponse()
            {
                Data = new GetUserDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    RoleName = user.UserRole.Role.ToString(),
                    Role = user.UserRole.Role,
                    PersonId = (await _personRepo.Get(x => x.UserId == user.Id))?.Id ?? 0,
                    AuthorizationCode = user.AuthorizationCode,
                },
                Message = "Login Successful",
                Status = true,
            };
            data.Token = _jWTAuthentication.GenerateToken(data.Data);
            data.Data.AuthorizationCode = null;
            return data;
        }
        return new UserLoginResponse()
        {
            Data = null,
            Message = "Invalid Username Or Password!",
            Status = false
        };
    }
    public async Task<UserLoginResponse> NFIDLogin(string token, string NFIDCode)
    {
        var user = await _userRepo.Get(x => x.NFIDCode == NFIDCode);
        var log = _authControl.CreateLog(user.Id);
        log.ActionType = "Attempted Login";
        log.LogDetails = $"{user.UserName} attempted to login via NFID";
        await _logService.CreateLog(log);
        if (_jWTAuthentication.ValidateHardwareToken(token) == true && user != null && BCrypt.Net.BCrypt.EnhancedVerify(NFIDCode, user.NFID))
        {
            return new UserLoginResponse()
            {
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
    public async Task<UserLoginResponse> GetByUserNameAuthCode(string userName)
    {
        var user = await _userRepo.GetUserByUserName(userName);
        if (user != null)
        {
            return new UserLoginResponse()
            {
                Data = new GetUserDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    RoleName = user.UserRole.Role.ToString(),
                    Role = user.UserRole.Role,
                    PersonId = (await _personRepo.Get(x => x.UserId == user.Id))?.Id ?? 0,
                },
                Message = "Login Successful",
                Status = true,
            };
        }
        return new UserLoginResponse()
        {
            Data = null,
            Message = "Invalid Username!",
            Status = false
        };
    }
    public async Task<BaseResponse> UpdateUserRole(UpdateUserDto updateUserDto)
    {
        var user = await _userRepo.GetUserById(updateUserDto.Id);
        if (user != null)
        {
            user.UserRole.Role = updateUserDto.Role;
            user.UserName = updateUserDto.UserName ?? user.UserName;
            user.LastModifiedOn = DateTime.Now;
            await _userRepo.Update(user);
            var log = _authControl.CreateLog(user.Id);
            log.ActionType = "User Role Updated";
            log.LogDetails = $"{user.UserName} updated their role to {user.UserRole.Role}.";
            await _logService.CreateLog(log);
            return new BaseResponse()
            {
                Message = "User Details Updated Successfully!",
                Status = true
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
        if (user != null && BCrypt.Net.BCrypt.EnhancedVerify(updateUserPasswordDto.AuthorizationCode, user.AuthorizationCode) && updateUserPasswordDto.Password == updateUserPasswordDto.NewPassword)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserPasswordDto.NewPassword);
            user.LastModifiedOn = DateTime.Now;
            await _userRepo.Update(user);
            var log = _authControl.CreateLog(user.Id);
            log.ActionType = "Password Changed";
            log.LogDetails = $"{user.UserName} changed their password!";
            await _logService.CreateLog(log);
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
            var log = _authControl.CreateLog(user.Id);
            log.ActionType = "Authorization Code Changed";
            log.LogDetails = $"{user.UserName} changed their authorization code!";
            await _logService.CreateLog(log);
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