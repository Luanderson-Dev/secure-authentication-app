using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureAuthApp.Application;
using SecureAuthApp.Domain;
using SecureAuthApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IUserRepository, UserRepository>();

var jwtSecret = builder.Configuration["Jwt:Secret"] 
                ?? throw new InvalidOperationException("Jwt:Secret não configurado no appsettings!");

builder.Services.AddScoped<AuthService>(provider => 
{
    var repository = provider.GetRequiredService<IUserRepository>();
    return new AuthService(repository, jwtSecret);
});

builder.Services.AddCors(o => o.AddPolicy("React", p => 
    p.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
));

var key = Encoding.ASCII.GetBytes(jwtSecret);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, ValidateAudience = false
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("access_token"))
                    context.Token = context.Request.Cookies["access_token"];
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseCors("React");
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
}

app.MapPost("/api/auth/register", (LoginRequest req, AuthService auth) =>
{
    try
    {
        return Results.Ok(new
        {
            message = auth.Register(req.Email, req.Password)
        });
    }
    catch (Exception e)
    {
        return Results.BadRequest(new { message = e.Message });
    }
});

app.MapPost("/api/auth/login", (LoginRequest req, AuthService auth, HttpContext context) =>
{
    try
    {
        var token = auth.Login(req.Email, req.Password);
        context.Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly =  true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddHours(2)
        });
        return Results.Ok(new { message = "Login successful" });
    }
    catch (Exception e)
    {
        return Results.Unauthorized();
    }
});

app.MapPost("/api/auth/logout", (HttpContext context) =>
{
    context.Response.Cookies.Delete("access_token");
    return Results.Ok(new { message = "Logged out successfully" });
}); 

app.MapGet("/api/protected", () =>
{
    return Results.Ok(new { message = "This is a protected endpoint" });
}).RequireAuthorization();

app.Run();

public record LoginRequest(string Email, string Password);