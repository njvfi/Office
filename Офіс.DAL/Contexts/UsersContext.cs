using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Офіс.DAL.Entities;


namespace Офіс.DAL.Contexts
{
    public class UsersContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Users> Users { get; set; } = null!;

        public UsersContext(DbContextOptions<UsersContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
