using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using Home_Security.Entities;
using Home_Security.Models.DTOs;
namespace Home_Security.Authentication;

public interface IJWTAuthentication
{
    public string GenerateToken(GetUserDto getUserDto);
    public GetUserDto? GetUserFromToken(string token);
    public Task<GetUserDto?> GetUserFromToken(HttpRequest token);
    public string AssignBrowserId(HttpRequest token);
    public string GenerateHardwareToken(GetHardwareDataDto getHardwareDataDto);
    public GetHardwareDataDto? ValidateHardwareToken(string token);
    public Task RefreshBrowserId(CancellationToken cancellationToken);
}
