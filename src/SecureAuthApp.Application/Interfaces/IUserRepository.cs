using SecureAuthApp.Domain.Entities;

namespace SecureAuthApp.Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}