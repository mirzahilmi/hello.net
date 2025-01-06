namespace Hello.NET.Extensions;

using System.Net;
using System.Threading.RateLimiting;
using Asp.Versioning;
using FluentValidation;
using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Repositories;
using Hello.NET.Domain.Services;
using Hello.NET.ExceptionHandlers;
using Hello.NET.Filters;
using Hello.NET.Infrastructure.SQL.Database;
using Hello.NET.Infrastructure.SQL.Repositories;
using Hello.NET.Usecase.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

public static class Extensions
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
        builder.Services.AddExceptionHandler<DataViolationExceptionHandler>();
        builder.Services.AddExceptionHandler<TimeOutExceptionHandler>();
        builder.Services.AddExceptionHandler<DefaultExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddControllers();
        builder.Services.AddResponseCaching();
    }

    public static void AddApplicationDependency(
        this IHostApplicationBuilder builder
    )
    {
        builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("PrimaryDatabase"),
                poolOptions => poolOptions.EnableRetryOnFailure(3)
            );
            options.EnableSensitiveDataLogging();
        });
        builder.Services.AddSerilog(options =>
            options.ReadFrom.Configuration(builder.Configuration)
        );
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddScoped<Transaction>();
        builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
        builder.Services.AddScoped<IArticleService, ArticleService>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<
            InputValidationFilter<ArticleCreateRequest>
        >();
    }
}
