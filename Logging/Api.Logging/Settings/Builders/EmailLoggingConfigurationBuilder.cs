using Serilog.Events;

namespace Api.Logging.Settings.Builders;

public sealed record EmailLoggingConfigurationBuilder(LoggingSettings LoggingSettings) : LoggingSettingsBuilder(LoggingSettings)
{
    public EmailLoggingConfigurationBuilder WithTemplate(string Template)
    {
        LoggingSettings.Email.Template = Template;

        return this;
    }

    public new EmailLoggingConfigurationBuilder WithLogLevel(LogEventLevel logLevel)
    {
        LoggingSettings.Email.LogLevel = logLevel;

        return this;
    }

    public EmailLoggingConfigurationBuilder WithSubject(string Subject)
    {
        LoggingSettings.Email.Subject = Subject;

        return this;
    }
}
