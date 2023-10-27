using Microsoft.EntityFrameworkCore;
using Models;

namespace WebApp.Server;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Role> Roles { get; set; } = default!;
    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<UserRole> UserRoles { get; set; } = default!;

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasMany(user => user.Roles)
                .WithMany(role => role.Users)
                .UsingEntity<UserRole>(
                    j => j
                        .HasOne(x => x.Role)
                        .WithMany()
                        .HasForeignKey(userRole => userRole.RoleId),
                    j => j
                        .HasOne(x => x.User)
                        .WithMany()
                        .HasForeignKey(userRole => userRole.UserId),
                    j => j.HasKey(t => new { t.UserId, t.RoleId })
                );
        });

        modelBuilder.Entity<Role>(builder =>
        {
            builder.HasOne(role => role.MatchedDepartment)
                .WithMany()
                .HasForeignKey(role => role.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}