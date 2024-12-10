using hello.net.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace hello.net.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }

        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }
    }
}
