using Api.Logging.Settings.Configs;
using Serilog.Events;

namespace Api.Logging.Settings;

public sealed record LoggingSettings
{
    public LogEventLevel LogLevel { get; set; } = LogEventLevel.Information;

    public string Template { get; set; }

    public EmailLoggingConfiguration Email { get; set; }
    public FileLoggingConfiguration File { get; set; }
    public SeqLoggingConfiguration Seq { get; set; }
    public GrayLogLoggingConfiguration GrayLog { get; set; }


    internal readonly Dictionary<string, string> Properties = new Dictionary<string, string>();
    public void WithProperty(string name, string value)
    {
        Properties[name] = value;
    }

    internal readonly Dictionary<string, LogEventLevel> Overrides = new Dictionary<string, LogEventLevel>();
    public void WithOverride(string name, LogEventLevel logEventLevel)
    {
        Overrides[name] = logEventLevel;
    }

}


