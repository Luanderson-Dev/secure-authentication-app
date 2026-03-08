using Microsoft.EntityFrameworkCore;
using SecureAuthApp.Application;
using SecureAuthApp.Application.Interfaces;
using SecureAuthApp.Domain.Entities;
using SecureAuthApp.Infrastructure.Persistence;

namespace SecureAuthApp.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}