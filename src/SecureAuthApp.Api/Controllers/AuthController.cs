using System.IdentityModel.Tokens.Jwt;
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
        
        string accessToken = jwtProvider.GenerateToken(user);
        string refreshToken = jwtProvider.GenerateRefreshToken();
        
        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
        await userRepository.UpdateAsync(user);
        
       SetTokenCookies(accessToken, refreshToken);
        return Ok(new {Message = "Login successful"});
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
        {
            return Unauthorized(new {Message = "Invalid refresh token"});
        }
        
        var accesToken = Request.Cookies["access_token"];
        if (string.IsNullOrEmpty(accesToken)) return Unauthorized();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(accesToken);
        var email = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Email).Value;
        
        var user = await userRepository.GetByEmailAsync(email);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Unauthorized(new {Message = "Session expired or invalid. Please log in again"});
        }
        
        string newAccessToken = jwtProvider.GenerateToken(user);
        string newRefreshToken = jwtProvider.GenerateRefreshToken();
        
        user.SetRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7));
        await userRepository.UpdateAsync(user);
        
        SetTokenCookies(newAccessToken, newRefreshToken);
        return Ok(new {Message = "Refresh successful"});
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("access_token");
        Response.Cookies.Delete("refresh_token");

        if (Request.Cookies.TryGetValue("access_token", out var accessToken))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

            if (email != null)
            {
                var user = await userRepository.GetByEmailAsync(email);
                if (user != null)
                {
                    user.RevokeRefreshToken();
                    await userRepository.UpdateAsync(user);
                }
            }
        }
        
        return Ok(new {Message = "Logged out successfully"});
    }

    private void SetTokenCookies(string accessToken, string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };

        var accessOptions = cookieOptions;
        accessOptions.Expires = DateTime.UtcNow.AddMinutes(15);
        Response.Cookies.Append("access_token", accessToken, cookieOptions);

        var refreshOptions = cookieOptions;
        refreshOptions.Expires = DateTime.UtcNow.AddDays(7);
        Response.Cookies.Append("refresh_token", refreshToken, cookieOptions);
    }
}