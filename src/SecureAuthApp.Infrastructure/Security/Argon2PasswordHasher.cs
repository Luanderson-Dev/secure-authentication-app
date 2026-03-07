using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using SecureAuthApp.Application;
using SecureAuthApp.Application.Interfaces;

namespace SecureAuthApp.Infrastructure.Security;

public class Argon2PasswordHasher : IPasswordHasher
{
    private readonly string _pepper;

    public Argon2PasswordHasher(IConfiguration configuration)
    {
        _pepper = configuration["PEPPER_SECRET"] 
                  ?? throw new ArgumentNullException("Pepper secret not found in configuration");
    }

    public string HashPassword(string password)
    {
        byte[] salt = CreateSalt();
        string passwordWithPepper = password + _pepper;

        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(passwordWithPepper))
        {
            Salt = salt,
            DegreeOfParallelism = 8,
            Iterations = 4,
            MemorySize = 1024 * 128
        };
        
        byte[] hash = argon2.GetBytes(32);
        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    public bool VerifyPassword(string password, string hash)
    {
        var parts = hash.Split(':');
        if (parts.Length != 2) return false;
        
        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] expcetedHash = Convert.FromBase64String(parts[1]);
        
        string passwordWithPepper = password + _pepper;
        
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(passwordWithPepper))
        {
            Salt = salt,
            DegreeOfParallelism = 8,
            Iterations = 4,
            MemorySize = 1024 * 128
        };
        
        byte[] actualHash = argon2.GetBytes(32);
        return CryptographicOperations.FixedTimeEquals(expcetedHash, actualHash);
    }

    private byte[] CreateSalt()
    {
        byte[] salt = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }
}