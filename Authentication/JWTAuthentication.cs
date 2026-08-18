using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Home_Security.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using Home_Security.Entities.Identity;
using Home_Security.Entities;
using Microsoft.Extensions.Options;
namespace Home_Security.Authentication;

public class JWTAuthentication : IJWTAuthentication
{
    public IConfiguration _key;
    public JWTAuthentication(IConfiguration config)
    {
        _key = config.GetSection("Jwt");
    }
    public string GenerateToken(GetUserDto getUserDto)
    {
        List<Claim> claims = new List<Claim>();
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(_key["SecretKey"] ?? "HomeSecurity1234567890");
        claims.Add(new Claim("UserId", getUserDto.Id.ToString()));
        claims.Add(new Claim("PersonId", getUserDto.PersonId.ToString()));
        claims.Add(new Claim("UserName", getUserDto.UserName.ToString()));
        claims.Add(new Claim("RoleName", getUserDto.RoleName));
        claims.Add(new Claim("AuthorizationCode", getUserDto.AuthorizationCode));
        claims.Add(new Claim("TimeStamp", DateTime.Now.ToString()));
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.Now,
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature),
            Audience = _key["Audience"] ?? "Home",
            Issuer = _key["Issuer"] ?? "Home_Security",
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    public GetUserDto? GetUserFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_key["SecretKey"] ?? "HomeSecurity1234567890");

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _key["Issuer"] ?? "Home_Security",
            ValidAudience = _key["Audience"] ?? "Home",

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

            var claims = principal.Claims;
            var timeStamp = DateTime.Parse(claims.First(x => x.Type == "TimeStamp").Value);
            if(timeStamp.AddDays(1) < DateTime.Now) return null;
            return new GetUserDto
            {
                Id = int.Parse(claims.First(x => x.Type == "UserId").Value),
                PersonId = int.Parse(claims.First(x => x.Type == "PersonId").Value),
                UserName = claims.First(x => x.Type == "UserName").Value,
                RoleName = claims.First(x => x.Type == "RoleName").Value,
                AuthorizationCode = claims.First(x => x.Type == "AuthorizationCode").Value
            };
        }
        catch
        {
            return null;
        }
    }
    public string GenerateHardwareToken(GetHardwareDataDto getHardwareDataDto)
    {
        var claims = new List<Claim>
        {
            new("hardwareId", getHardwareDataDto.HardwareId),
            new("authKey", getHardwareDataDto.AuthKey),
            new Claim("UserId", getHardwareDataDto.Id.ToString()),
            new Claim("PersonId", getHardwareDataDto.PersonId.ToString()),
            new Claim("UserName", getHardwareDataDto.UserName.ToString()),
            new Claim("RoleName", getHardwareDataDto.RoleName),
            new Claim("AuthorizationCode", getHardwareDataDto.AuthorizationCode),
            new Claim("TimeStamp", DateTime.Now.ToString())
        };

        var credentials = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key["SecretKey"] ?? "HomeSecurity1234567890")),
        SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.MaxValue,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public GetHardwareDataDto ValidateHardwareToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_key["SecretKey"] ?? "HomeSecurity1234567890");

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        try
        {
            if (string.IsNullOrEmpty(token))
                return null;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var jwtToken = ((JwtSecurityToken)validatedToken).Claims;
            return new GetHardwareDataDto
            {
                Id = int.Parse(jwtToken.First(x => x.Type == "UserId").Value),
                PersonId = int.Parse(jwtToken.First(x => x.Type == "PersonId").Value),
                UserName = jwtToken.First(x => x.Type == "UserName").Value,
                RoleName = jwtToken.First(x => x.Type == "RoleName").Value,
                AuthorizationCode = jwtToken.First(x => x.Type == "AuthorizationCode").Value,
                HardwareId = jwtToken.First(x => x.Type == "HardwareId").Value,
                AuthKey = jwtToken.First(x => x.Type == "AuthKey").Value,
                Status = true
            };
        }
        catch (SecurityTokenExpiredException)
        {
            return null;
        }
        catch
        {
            return null;
        }
    }
}
public static class ClaimsPrincipalExtensions
{
    public static GetAuthControlInfoDto? GetCurrentUserInfo(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var personIdClaim = user.FindFirst("PersonId")?.Value;
        var authorizationCode = user.FindFirst(ClaimTypes.Authentication)?.Value;

        if (!int.TryParse(userIdClaim, out int userId) ||
            !int.TryParse(personIdClaim, out int personId))
        {
            return null;
        }

        return new GetAuthControlInfoDto
        {
            PersonId = personId,
            AuthorizationCode = authorizationCode ?? ""
        };
    }
}