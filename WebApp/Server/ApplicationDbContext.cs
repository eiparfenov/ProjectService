using Microsoft.EntityFrameworkCore;
using Models;

namespace WebApp.Server;

public class ApplicationDbContext: DbContext
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Role> Roles { get; set; } = default!;
    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<UserRole> UserRoles { get; set; } = default!;

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        if (Database.EnsureCreated())
        {
            var userId = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            var department = new Department()
            {
                Id = Guid.NewGuid(),
                Title = "rtu-tc"
            };
            var superAdminRole = new Role()
            {
                Id = roleId,
                RoleType = RoleType.SuperAdmin
            };
            var user = new User()
            {
                Id = userId,
                VkId = 245257490
            };
          

            Users.Add(user);
            Roles.Add(superAdminRole);
            user.Roles = new List<Role>() { superAdminRole };
            Departments.Add(department);
            SaveChanges();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(user => user.Roles)
            .WithMany(role => role.Users)
            .UsingEntity<UserRole>(
                j => j
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey(userRole => userRole.UserId),
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey(userRole => userRole.RoleId),
                j =>
                {
                    j.HasKey(t => new { t.UserId, t.RoleId });
                }

            );
    }
}