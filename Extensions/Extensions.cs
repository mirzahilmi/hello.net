using Hello.NET.Data;
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
    }
}
