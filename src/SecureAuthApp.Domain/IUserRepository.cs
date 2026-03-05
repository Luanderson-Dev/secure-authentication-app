namespace SecureAuthApp.Domain;

public interface IUserRepository
{
    void Add(User user);
    User? GetByEmail(string email);
    void SaveChanges();
}