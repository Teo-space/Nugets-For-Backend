using Serilog.Events;

namespace Api.Logging.Settings.Configs;

public sealed record SeqLoggingConfiguration
{
    public LogEventLevel LogLevel { get; set; } = LogEventLevel.Information;

    public string ServerUrl { get; set; }
    public string ApiKey { get; set; }

}