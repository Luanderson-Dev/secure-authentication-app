using Microsoft.EntityFrameworkCore;
using SecureAuthApp.Application;
using SecureAuthApp.Application.Interfaces;
using SecureAuthApp.Domain.Entities;
using SecureAuthApp.Infrastructure.Persistence;

namespace SecureAuthApp.Infrastructure.Repositories;

public class UserRepository: IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }
}