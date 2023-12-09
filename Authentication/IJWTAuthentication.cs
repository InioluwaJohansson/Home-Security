using System.Security.Claims;
using System.Text;
using Home_Security.Models.DTOs;
namespace Home_Security.Authentication;

public interface IJWTAuthentication
{
    string GenerateToken(GetUserDto getUserDto);
}
