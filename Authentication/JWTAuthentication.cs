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
    public string _key;
    public JWTAuthentication(IConfiguration config)
    {
        _key = config.GetSection("Jwt:SecretKey").Value ?? "HomeSecurity1234567890";
    }
    public string GenerateToken(GetUserDto getUserDto)
    {
        List<Claim> claims = new List<Claim>();
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(_key);
        claims.Add(new Claim("UserId", getUserDto.Id.ToString()));
        claims.Add(new Claim("PersonId", getUserDto.PersonId.ToString()));
        claims.Add(new Claim("UserName", getUserDto.UserName.ToString()));
        claims.Add(new Claim("RoleName", getUserDto.RoleName));
        claims.Add(new Claim("AuthorizationCode", getUserDto.AuthorizationCode));
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.Now,
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature),
            Audience = "Home",
            Issuer = "Owner",
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    public GetUserDto? GetUserFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_key);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "Owner",
            ValidAudience = "Home",

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

            var claims = principal.Claims;

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
    public string GenerateHardwareToken(string hardwareId, string AuthKey)
    {
        var claims = new List<Claim>
        {
            new("hardwareId", hardwareId),
            new("authKey", AuthKey)
        };

        var credentials = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
        SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public bool ValidateHardwareToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_key);

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
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var hardwareId = jwtToken.Claims.FirstOrDefault(c => c.Type == "hardwareId")?.Value;
            var authKey = jwtToken.Claims.FirstOrDefault(c => c.Type == "authKey")?.Value;
            if (string.IsNullOrEmpty(hardwareId) || string.IsNullOrEmpty(authKey))
                return false;
            return true;
        }
        catch (SecurityTokenExpiredException)
        {
            return false;
        }
        catch
        {
            return false;
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