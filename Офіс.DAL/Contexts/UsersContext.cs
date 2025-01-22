using Microsoft.EntityFrameworkCore;
using Офіс.DAL.Entities;


namespace Офіс.DAL.Contexts
{
    public class UsersContext : DbContext
    {
        public DbSet<Users> Users { get; set; } = null!;

        public UsersContext(DbContextOptions<UsersContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
