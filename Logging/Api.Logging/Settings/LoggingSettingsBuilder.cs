using Api.Logging.Settings.Builders;
using Api.Logging.Settings.Configs;
using Serilog.Events;

namespace Api.Logging.Settings;

public record LoggingSettingsBuilder(LoggingSettings LoggingSettings)
{
    public EmailLoggingConfigurationBuilder AddEmailLogging(string From, IReadOnlyCollection<string> To, string Host, int Port = 25)
    {
        LoggingSettings.Email = new EmailLoggingConfiguration
        {
            From = From,
            To = To,
            Host = Host,
            Port = Port,
        };

        return new EmailLoggingConfigurationBuilder(LoggingSettings);
    }

    public FileLoggingConfigurationBuilder AddFileLogging(string Path, string Template = default)
    {
        LoggingSettings.File = new FileLoggingConfiguration
        {
            Template = Template,
            Path = Path
        };

        return new FileLoggingConfigurationBuilder(LoggingSettings);
    }

    public SeqLoggingConfigurationBuilder AddSeqLogging(string ServerUrl, string ApiKey)
    {
        LoggingSettings.Seq = new SeqLoggingConfiguration
        {
            ServerUrl = ServerUrl,
            ApiKey = ApiKey
        };

        return new SeqLoggingConfigurationBuilder(LoggingSettings);
    }

    public GrayLogLoggingConfigurationBuilder AddGrayLog(string stream, string host, int port)
    {
        LoggingSettings.GrayLog = new GrayLogLoggingConfiguration
        {
            Stream = stream,
            Host = host,
            Port = port
        };

        return new GrayLogLoggingConfigurationBuilder(LoggingSettings);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public LoggingSettingsBuilder WithProperty(string name, string value)
    {
        LoggingSettings.WithProperty(name, value);

        return this;
    }

    public LoggingSettingsBuilder WithLogLevel(LogEventLevel logLevel)
    {
        LoggingSettings.LogLevel = logLevel;

        return this;
    }

    public LoggingSettingsBuilder WithOverride(string name, LogEventLevel logEventLevel)
    {
        LoggingSettings.WithOverride(name, logEventLevel);

        return this;
    }
}
