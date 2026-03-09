using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using SecureAuthApp.Infrastructure.Security;

namespace SecureAuthApp.Tests.Security;

public class Argon2PasswordHasherTests
{
    private readonly Argon2PasswordHasher _sut;

    public Argon2PasswordHasherTests()
    {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["PEPPER_SECRET"]).Returns("TestPepper123!");
        _sut = new Argon2PasswordHasher(configMock.Object);
    }

    [Fact]
    public void HashPassword_ShouldReturnHashWithSalt()
    {
        string hash = _sut.HashPassword("StrongPassword123");

        hash.Should().NotBeNullOrWhiteSpace();
        hash.Should().Contain(":");
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        string password = "StrongPassword123";
        string storedHash = _sut.HashPassword(password);
        
        bool isValid = _sut.VerifyPassword(password, storedHash);
        
        isValid.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_WithWrongPassword_ShouldReturnFalse()
    {
        string storedHash = _sut.HashPassword("CorrectPassword");
        bool isValid = _sut.VerifyPassword("WrongPassword", storedHash);
        isValid.Should().BeFalse();
    }
}