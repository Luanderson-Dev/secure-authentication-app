using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SecureAuthApp.Domain;

namespace SecureAuthApp.Application;

public class AuthService
{
    private readonly IUserRepository _repository;
    private readonly string _jwtSecret;
    
    public AuthService(IUserRepository repository, string jwtSecret)
    {
        _repository = repository;
        _jwtSecret = jwtSecret;
    }
    
    public string Register(string email, string password)
    {
        if (_repository.GetByEmail(email) != null) 
            throw new Exception("Email already in use");

        string hash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User(email, hash);
        
        _repository.Add(user);
        _repository.SaveChanges();
        
        return "User registered successfully";
    }

    public string Login(string email, string password)
    {
        var user = _repository.GetByEmail(email);
        if(user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Invalid credentials");
        
        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, user.Email) }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}