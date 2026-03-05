using Microsoft.EntityFrameworkCore;
using SecureAuthApp.Domain;

namespace SecureAuthApp.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}
    public DbSet<User> Users { get; set; }
}