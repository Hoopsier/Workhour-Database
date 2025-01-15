using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Työtunnit_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
    }
}
