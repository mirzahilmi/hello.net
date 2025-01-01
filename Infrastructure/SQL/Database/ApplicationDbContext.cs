namespace Hello.NET.Infrastructure.SQL.Database;

using Hello.NET.Infrastructure.SQL.Database.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ArticleEntity> Articles { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
}
