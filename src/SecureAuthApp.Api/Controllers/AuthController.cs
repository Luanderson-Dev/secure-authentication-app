using Microsoft.AspNetCore.Mvc;
using SecureAuthApp.Application.DTOs;
using SecureAuthApp.Application.Interfaces;
using SecureAuthApp.Domain.Entities;

namespace SecureAuthApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return BadRequest(new {Message = "Email already in use"});
        }
        
        var hashedPassword = passwordHasher.HashPassword(request.Password);

        var newUser = new User(request.Email, hashedPassword);
        await userRepository.AddAsync(newUser);
        
        return Ok(new {Message = "User registered successfully"});
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null || !passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Unauthorized(new {Message = "Invalid email or password"});
        }
        
        bool isPasswordValid = passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return Unauthorized(new { Message = "E-mail ou senha incorretos." });
        }

        string token = jwtProvider.GenerateToken(user);
        
        var cookiesOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(2)
        };
        
        Response.Cookies.Append("access_token", token, cookiesOptions);
        return Ok(new {Message = "Login successful"});
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        return Ok(new {Message = "Logged out successfully"});
    }
}