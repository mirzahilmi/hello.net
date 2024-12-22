namespace Hello.NET.Data;

using Hello.NET.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Article> Articles { get; set; }

    public ApplicationDbContext(DbContextOptions options)
        : base(options) { }
}
