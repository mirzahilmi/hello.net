using Microsoft.Extensions.Options;

namespace Hello.NET.Options;

public sealed class OpenTelemetryOptionsSetup(IConfiguration configuration)
    : IConfigureOptions<OpenTelemetryOptions>
{
    public const string SectionName = "OpenTelemetry";
    public readonly IConfiguration _configuration = configuration;

    public void Configure(OpenTelemetryOptions options) {
        _configuration.GetSection(SectionName).Bind(options);
    }
}
