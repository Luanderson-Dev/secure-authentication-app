namespace SecureAuthApp.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected User() {}

    public User(string email, string passwordHash, string role = "User")
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }
}
