using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Controls;
public class TokenControl : ITokenControl
{
    IAuthControl _authControl;
    ITokenService _tokenService;
    public TokenControl(IAuthControl authControl, ITokenService tokenService)
    {
        _authControl = authControl;
        _tokenService = tokenService;
    }
    public async Task<BaseResponse> GenerateToken(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var token = await _tokenService.GenerateToken(auth.Id);
            if (token.Status == true)  return token;
        }
        return _authControl.AuthFaliure();
    }
}