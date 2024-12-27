using Hello.NET.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddApplicationDependency();
builder.AddApplicationConfiguration();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}
app.UseExceptionHandler();
app.UseResponseCaching();
app.MapControllers();
app.Run();
