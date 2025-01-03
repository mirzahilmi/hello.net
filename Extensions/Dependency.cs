namespace Hello.NET.Extensions;

using FluentValidation;
using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Repositories;
using Hello.NET.Domain.Services;
using Hello.NET.Filters;
using Hello.NET.Infrastructure.SQL.Database;
using Hello.NET.Infrastructure.SQL.Repositories;
using Hello.NET.Usecase.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

public static class Dependency
{
    public static void AddApplicationDependency(
        this IHostApplicationBuilder builder
    )
    {
        builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("PrimaryDatabase"),
                poolOptions => poolOptions.EnableRetryOnFailure(3)
            )
        );
        builder.Services.AddSerilog(options =>
            options.ReadFrom.Configuration(builder.Configuration)
        );
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
        builder.Services.AddScoped<IArticleService, ArticleService>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<InputValidationFilter<ArticleDto>>();
    }
}
