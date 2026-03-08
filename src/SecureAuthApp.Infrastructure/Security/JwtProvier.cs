using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SecureAuthApp.Application.Interfaces;
using SecureAuthApp.Domain.Entities;

namespace SecureAuthApp.Infrastructure.Security;

public class JwtProvier(IConfiguration configuration) : IJwtProvider
{
    private readonly string _jwtSecret = configuration["JWT_SECRET"] 
                                         ?? throw new ArgumentNullException("JWT secret not found in configuration");

    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("role", user.Role),
        };

        var tokenDescriptor = new JwtSecurityToken(
            issuer: "SecureAuthApp", 
            audience: "SecureAuthAppClient",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}