using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using SecureAuthApp.Domain.Entities;
using SecureAuthApp.Infrastructure.Security;

namespace SecureAuthApp.Tests.Security;

public class JwtProviderTests
{
    private readonly JwtProvider _sut;

    public JwtProviderTests()
    {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["JWT_SECRET"]).Returns("VeryLongStringKeyForJwtUnitTests2026!");
        _sut = new JwtProvider(configMock.Object);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwtString_WithUserClaims()
    {
        var user = new User("admin@test.com", "hash", "Admin");
        
        string token = _sut.GenerateToken(user);
        token.Should().NotBeNullOrWhiteSpace();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        
        jwtToken.Issuer.Should().Be("SecureAuthApp");
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == "admin@test.com");
        jwtToken.Claims.Should().Contain(c => c.Type == "role" && c.Value == "Admin");
    }
}