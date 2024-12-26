namespace Hello.NET.Extensions;

using FluentValidation;
using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Services;
using Hello.NET.Filters;
using Hello.NET.Infrastructure.SQL.Database;
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
                builder.Configuration.GetConnectionString("PrimaryDatabase")
            )
        );
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddScoped<IArticleMapper, ArticleMapper>();
        builder.Services.AddScoped<IArticleService, ArticleService>();
        builder.Services.AddScoped<InputValidationFilter<ArticleDto>>();
    }
}
