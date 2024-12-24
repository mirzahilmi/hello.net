using Asp.Versioning;
using FluentValidation;
using Hello.NET.Data;
using Hello.NET.Domain.Services;
using Hello.NET.Mapping;
using Hello.NET.Mapping.Interfaces;
using Hello.NET.Options;
using Microsoft.EntityFrameworkCore;

namespace Hello.NET.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(
        this WebApplicationBuilder builder
    )
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("PrimaryDatabase")
            )
        );
        builder.Services.ConfigureOptions<OpenTelemetryOptionsSetup>();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddScoped<IArticleMapper, ArticleMapper>();
        builder.Services.AddScoped<IArticleService, ArticleService>();
        builder.Services.AddCors(options =>
            options.AddPolicy(
                "AllowAll",
                _builder =>
                {
                    _builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                }
            )
        );
        builder.Services.AddResponseCaching();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder
            .Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1.0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version", "X-API-Version")
                );
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
    }
}
