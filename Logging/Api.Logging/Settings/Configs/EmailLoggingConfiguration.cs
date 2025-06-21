using Serilog.Events;

namespace Api.Logging.Settings.Configs;

public sealed record EmailLoggingConfiguration
{
    public string Subject { get; set; }
    public string Template { get; set; }

    public LogEventLevel LogLevel { get; set; } = LogEventLevel.Error;

    public string From { get; set; }
    public IReadOnlyCollection<string> To { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}
