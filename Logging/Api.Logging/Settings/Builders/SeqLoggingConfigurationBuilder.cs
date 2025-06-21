using Serilog.Events;

namespace Api.Logging.Settings.Builders;

public sealed record SeqLoggingConfigurationBuilder(LoggingSettings LoggingSettings) : LoggingSettingsBuilder(LoggingSettings)
{
    public new SeqLoggingConfigurationBuilder WithLogLevel(LogEventLevel logLevel)
    {
        LoggingSettings.Seq.LogLevel = logLevel;

        return this;
    }
}