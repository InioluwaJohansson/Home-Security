using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Home_Security.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using Home_Security.Entities.Identity;
using Home_Security.Entities;
namespace Home_Security.Authentication;
public class JWTAuthentication : IJWTAuthentication
{
    public string _key;
    public JWTAuthentication(string key)
    {
        _key = key;
    }
    public string GenerateToken(GetUserDto getUserDto)
    {
        List<Claim> claims = new List<Claim>();
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(_key);
        claims.Add(new Claim(ClaimTypes.Name, getUserDto.Id.ToString()));
        claims.Add(new Claim(ClaimTypes.NameIdentifier, getUserDto.PersonId.ToString()));
        claims.Add(new Claim(ClaimTypes.Actor, getUserDto.UserName.ToString()));
        claims.Add(new Claim(ClaimTypes.Role, getUserDto.RoleName));
        claims.Add(new Claim(ClaimTypes.Authentication, getUserDto.AuthorizationCode));
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
    public string GenerateHardwareToken(Hardware hardware)
    {
        var claims = new List<Claim>
        {
            new("hardwareId", hardware.HardwareId),
            new("authKey", hardware.AuthKey)
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
}