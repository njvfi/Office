using Microsoft.EntityFrameworkCore;
using Офіс.DAL.Entities;

namespace Офіс.DAL.Contexts
{
    public class EventsContext : DbContext
    {
        public DbSet<Events> Events { get; set; } = null!;
        public EventsContext(DbContextOptions<EventsContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
