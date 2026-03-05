using SecureAuthApp.Domain;

namespace SecureAuthApp.Infrastructure;

public class UserRepository: IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public void Add(User user) => _context.Users.Add(user);
    public User? GetByEmail(string email) => _context.Users.FirstOrDefault(u => u.Email == email);
    public void SaveChanges() => _context.SaveChanges();
}