using Microsoft.EntityFrameworkCore;
using TaskLabBackend.Models;
using TaskLabBackend.Models.Api;

namespace TaskLabBackend.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Models.Task> Tasks { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
    

}
