using Hello.NET.Data;
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
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("PrimaryDatabase")
            )
        );
        builder.Services.ConfigureOptions<OpenTelemetryOptionsSetup>();
        builder.Services.AddScoped<IArticleDtoMapper, ArticleDtoMapper>();
    }
}
