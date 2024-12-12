namespace hello.net.Data;

using hello.net.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Article> Articles { get; set; }

    public ApplicationDbContext(DbContextOptions options)
        : base(options) { }
}
