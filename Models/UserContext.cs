using Microsoft.EntityFrameworkCore;

namespace Työtunnit_API.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
