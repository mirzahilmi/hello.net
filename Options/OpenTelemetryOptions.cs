namespace Hello.NET.Options;

public class OpenTelemetryOptions
{
    public required string ServiceName { get; init; }
    public required string ServiceVersion { get; init; }
}
