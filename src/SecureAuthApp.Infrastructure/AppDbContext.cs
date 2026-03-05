using Microsoft.EntityFrameworkCore;

namespace SecureAuthApp.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}
}