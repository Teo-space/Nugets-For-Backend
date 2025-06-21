using Serilog.Events;

namespace Api.Logging.Settings.Builders;

public sealed record GrayLogLoggingConfigurationBuilder(LoggingSettings LoggingSettings) : LoggingSettingsBuilder(LoggingSettings)
{
    public new GrayLogLoggingConfigurationBuilder WithLogLevel(LogEventLevel logLevel)
    {
        LoggingSettings.GrayLog.LogLevel = logLevel;

        return this;
    }

}