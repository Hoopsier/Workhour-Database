using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Runtime.InteropServices;

namespace Työtunnit_API.Models
{
    public class Workhour
    {
        public int Id { get; set; }
        public int UId { get; set; }
        public string UserName { get; set; }
        public string? Details { get; set; }
        public int Hours { get; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
    }
}
