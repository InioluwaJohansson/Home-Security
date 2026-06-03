using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Services;
public class TokenService : ITokenService
{
    ITokenRepo _tokenRepo;
    public TokenService(ITokenRepo tokenRepo)
    {
        _tokenRepo = tokenRepo;
    }
    public async Task<GetTokenDto> GenerateToken(int personId)
    {
        var token = new Token()
        {
            TokenCode = Guid.NewGuid().ToString().Substring(0,30).Replace("-",""),
            ExpiryTime = DateTime.Now.AddMinutes(5),
            IsDeleted = false,
            CreatedBy = personId,
            LastModifiedBy = personId,
            LastModifiedOn = DateTime.Now,
            CreatedOn = DateTime.Now
        };
        await _tokenRepo.Create(token);
        return new GetTokenDto()
        {
            TokenCode = token.TokenCode,
            ExpiryTime = token.ExpiryTime,
            Status = true,
            Message = "Token Successfully Generated!"
        };
    }
    public async Task<BaseResponse> VerifyToken(string tokenCode)
    {
        if (tokenCode != null)
        {
            Console.WriteLine($"Received token: {tokenCode}");
            var token = await _tokenRepo.Get(x => x.TokenCode == tokenCode);
            if(token == null)
            {
                Console.WriteLine("Token not found in database.");
                return new BaseResponse()
                {
                    Status = false,
                    Message = "Invalid Token!"
                };
            }
            token.IsDeleted = true;
            await _tokenRepo.Update(token);
            return new BaseResponse()
            {
                Status = true,
                Message = "Token Verified!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Invalid Token!"
        };
    }
    
    public async Task<BaseResponse> DeleteAllTokens()
    {
        var tokens = await _tokenRepo.GetByExpression(x => x.IsDeleted == true || x.ExpiryTime <= DateTime.Now);
        if(tokens != null)
        {
            foreach(var token in tokens) await _tokenRepo.Delete(token);
        }
        return new BaseResponse()
        {
            Status = true,
            Message = "Inactive tokens deleted!"
        };
    }
}