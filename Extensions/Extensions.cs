using Hello.NET.Data;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Hello.NET.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(
        this WebApplicationBuilder builder
    )
    {
        var otlpConfig = builder.Configuration.GetSection("OpenTelemetry");
        var serviceName = otlpConfig.GetValue<string>("ServiceName")!;
        var serviceVersion = otlpConfig.GetValue<string>("ServiceVersion")!;
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("PrimaryDatabase")
            )
        );
        builder
            .Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
                resource.AddService(
                    serviceName: serviceName,
                    serviceVersion: serviceVersion
                )
            )
            .WithTracing(tracing =>
                tracing.AddAspNetCoreInstrumentation().AddConsoleExporter()
            );
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("PrimaryDatabase")
            )
        );
        builder.Logging.AddJsonConsole();
    }
}
