using Serilog.Events;

namespace Api.Logging.Settings.Builders;

public sealed record FileLoggingConfigurationBuilder(LoggingSettings LoggingSettings) : LoggingSettingsBuilder(LoggingSettings)
{
    public FileLoggingConfigurationBuilder WithTemplate(string Template)
    {
        LoggingSettings.File.Template = Template;

        return this;
    }

    public new FileLoggingConfigurationBuilder WithLogLevel(LogEventLevel logLevel)
    {
        LoggingSettings.File.LogLevel = logLevel;

        return this;
    }
}