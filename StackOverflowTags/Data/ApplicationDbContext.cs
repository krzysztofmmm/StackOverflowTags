using Microsoft.EntityFrameworkCore;
using StackOverflowTags.Models;

namespace StackOverflowTags.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Tag> Tags { get; set; }
    }
}
