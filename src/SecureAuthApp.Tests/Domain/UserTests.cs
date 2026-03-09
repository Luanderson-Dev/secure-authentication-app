using FluentAssertions;
using SecureAuthApp.Domain.Entities;

namespace SecureAuthApp.Tests.Domain;

public class UserTests
{
    [Fact]
    public void Contructor_ShouldCreateUser_WithValidDataAndDefaultRole()
    {
        string email = "teste@example.com";
        string passwordHash = "hashed_password_123";
        
        var user = new User(email, passwordHash);

        user.Id.Should().NotBeEmpty();
        user.Email.Should().Be(email);
        user.PasswordHash.Should().Be(passwordHash);
        user.Role.Should().Be("User");
        user.RefreshToken.Should().BeNull();
    }

    [Fact]
    public void SetRefreshToken_ShouldUpdateTokenAndExpiryTime()
    {
        var user = new User("teste@example.com", "hash");
        string refreshToken = "secure_token_123";
        DateTime expiryTime = DateTime.UtcNow.AddDays(7);
        
        user.SetRefreshToken(refreshToken, expiryTime);
        
        user.RefreshToken.Should().Be(refreshToken);
        user.RefreshTokenExpiryTime.Should().Be(expiryTime);
    }

    [Fact]
    public void RevokeRefreshToken_ShouldClearTokenData()
    {
        var user = new User("test@example.com", "hash");
        user.SetRefreshToken("old_token", DateTime.UtcNow.AddDays(1));
        
        user.RevokeRefreshToken();

        user.RefreshToken.Should().BeNull();
        user.RefreshTokenExpiryTime.Should().BeNull();
    }
}