namespace Hello.NET.Options;

public sealed class OpenTelemetryOptions
{
    public required string ServiceName { get; init; }
    public required string ServiceVersion { get; init; }
}
