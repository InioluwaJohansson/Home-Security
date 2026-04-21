using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Services;
public interface ITokenService
{
    public Task<GetTokenDto> GenerateToken();
    public Task<BaseResponse> CheckAllTokens();
}