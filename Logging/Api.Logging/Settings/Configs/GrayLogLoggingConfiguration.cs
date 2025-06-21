using Serilog.Events;

namespace Api.Logging.Settings.Configs;

public sealed record GrayLogLoggingConfiguration
{
    public LogEventLevel LogLevel { get; set; } = LogEventLevel.Information;

    public string Stream { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}
