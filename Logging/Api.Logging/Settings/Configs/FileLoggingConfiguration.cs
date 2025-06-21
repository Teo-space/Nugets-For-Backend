using Serilog.Events;

namespace Api.Logging.Settings.Configs;

public sealed record FileLoggingConfiguration
{
    public string Template { get; set; }

    public LogEventLevel LogLevel { get; set; } = LogEventLevel.Information;

    public string Path { get; set; }
}