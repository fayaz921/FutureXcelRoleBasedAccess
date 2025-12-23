using FutureXcelRoleBasedAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FutureXcelRoleBasedAccess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial admin user with FIXED values
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    Username = "fayaz",
                    Email = "mfayaz21703@gmail.com",
                    PasswordHash = "$2a$11$6XvVlE7LvCNEKnNkZ3hTZOqKqVQXJ7HwJL1YvD8TnxKL9rJZGxJ6K", // This is "fayaz921" hashed
                    Role = "Admin",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                }
            );
        }
    }
}
