namespace Hello.NET.Extensions;

using System.Net;
using System.Threading.RateLimiting;
using Asp.Versioning;

public static class Application
{
    public static void AddApplicationConfiguration(
        this IHostApplicationBuilder builder
    )
    {
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
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        builder.Services.AddCors(options =>
            options.AddPolicy(
                "AllowAll",
                _builder =>
                {
                    _builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                }
            )
        );
        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = (int)HttpStatusCode.TooManyRequests;
            options.OnRejected = async (context, token) =>
            {
                await context.HttpContext.Response.WriteAsync(
                    "Too many requests. Please try again later.",
                    cancellationToken: token
                );
            };
            options.GlobalLimiter = PartitionedRateLimiter.Create<
                HttpContext,
                string
            >(context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    // HACK: null-forgiving possible null reference
                    // TODO: i dont forgive anybody, fix this magic
                    partitionKey: context.Connection.RemoteIpAddress!.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 20,
                        Window = TimeSpan.FromSeconds(15),
                    }
                )
            );
        });
        builder.Services.AddControllers();
        builder.Services.AddResponseCaching();
    }
}
