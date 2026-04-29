using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Services;
public interface ITokenService
{
    public Task<GetTokenDto> GenerateToken(int personId);
    public Task<BaseResponse> VerifyToken(string tokenCode);
    public Task<BaseResponse> DeleteAllTokens();
}