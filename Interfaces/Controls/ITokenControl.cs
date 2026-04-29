using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface ITokenControl
{
    public Task<BaseResponse> GenerateToken(GetAuthControlInfoDto getAuthControlInfoDto);
}