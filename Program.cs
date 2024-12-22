using Hello.NET.Extensions;
using Hello.NET.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureOptions<OpenTelemetryOptionsSetup>();
builder.AddApplicationServices();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();
