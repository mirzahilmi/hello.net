namespace Hello.NET.Extensions;

using FluentValidation;
using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Repositories;
using Hello.NET.Filters;
using Hello.NET.Infrastructure.SQL.Database;
using Hello.NET.Infrastructure.SQL.Repositories;
using Hello.NET.Mapping;
using Hello.NET.Mapping.Interfaces;
using Microsoft.EntityFrameworkCore;

public static class Dependency
{
    public static void AddApplicationDependency(
        this IHostApplicationBuilder builder
    )
    {
        builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("PrimaryDatabase"),
                poolOptions =>
                {
                    poolOptions.EnableRetryOnFailure(3);
                }
            )
        );
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddScoped<IArticleMapper, ArticleMapper>();
        builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
        builder.Services.AddScoped<InputValidationFilter<ArticleDto>>();
    }
}
