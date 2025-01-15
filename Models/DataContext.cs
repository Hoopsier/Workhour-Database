using Microsoft.EntityFrameworkCore;

namespace Työtunnit_API.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Workhour> Workhours { get; set; }
    }
}
