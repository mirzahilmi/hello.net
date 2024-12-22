using Hello.NET.Extensions;
using Hello.NET.Mapping;
using Hello.NET.Mapping.Interfaces;
using Hello.NET.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureOptions<OpenTelemetryOptionsSetup>();
builder.Services.AddScoped<IArticleDtoMapper, ArticleDtoMapper>();
builder.AddApplicationServices();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();
