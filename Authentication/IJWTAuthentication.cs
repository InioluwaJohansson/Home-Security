using System.Security.Claims;
using System.Text;
using Home_Security.Entities;
using Home_Security.Models.DTOs;
namespace Home_Security.Authentication;

public interface IJWTAuthentication
{
    public string GenerateToken(GetUserDto getUserDto);
    public string GenerateHardwareToken(Hardware hardware);
}
