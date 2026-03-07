using SecureAuthApp.Domain.Entities;

namespace SecureAuthApp.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}