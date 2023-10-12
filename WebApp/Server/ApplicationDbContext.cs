using Microsoft.EntityFrameworkCore;
using Models;

namespace WebApp.Server;

public class ApplicationDbContext: DbContext
{
    public DbSet<User> Users { get; set; } = default!;

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }
}