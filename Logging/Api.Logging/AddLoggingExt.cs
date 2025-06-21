using Api.Logging;
using Api.Logging.Enrichers.Extensions;
using Api.Logging.Settings;
using Api.Logging.Settings.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Email;
using Serilog.Sinks.Graylog;

public static class AddLoggingExtensions
{
    public static IHostApplicationBuilder AddSerilogLogging(this IHostApplicationBuilder builder,
        string appName, Action<LoggingSettingsBuilder> options = default)
    {
        AddSerilogLogging(builder.Logging, builder.Services, builder.Configuration, appName, options);

        return builder;
    }

    public static ILoggingBuilder AddSerilogLogging(this ILoggingBuilder loggingBuilder,
        IServiceCollection services,
        IConfiguration configuration, string appName,
        Action<LoggingSettingsBuilder> options = default)
    {
        loggingBuilder.ClearProviders();

        IConfigurationSection loggingSettingsSection = configuration.GetSection(nameof(LoggingSettings));

        services.AddOptions<LoggingSettings>().Bind(loggingSettingsSection);

        LoggingSettings loggingSettings = loggingSettingsSection?.Get<LoggingSettings>() ?? new LoggingSettings();
        LoggingSettingsBuilder loggingSettingsBuilder = new LoggingSettingsBuilder(loggingSettings);

        if (options is not null)
        {
            options(loggingSettingsBuilder);
        }

        services.AddSerilog((IServiceProvider serviceProvider, LoggerConfiguration loggerConfiguration) =>
        {
            Configure(loggingBuilder, services, configuration, serviceProvider, appName, loggerConfiguration, loggingSettings);
        });

        return loggingBuilder;
    }

    private static void Configure(
        ILoggingBuilder loggingBuilder, IServiceCollection services, IConfiguration configuration, IServiceProvider serviceProvider,
        string appName, LoggerConfiguration loggerConfiguration, LoggingSettings loggingSettings)
    {
        loggerConfiguration.ReadFrom.Configuration(configuration).ReadFrom.Services(serviceProvider)
        .Enrich.WithProperty("Application", appName)

        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnv()
        ;

        if (CheckEnvironment.IsDevelopment)
        {
            loggerConfiguration.MinimumLevel.Debug();

            loggerConfiguration.WriteTo.Console(outputTemplate: loggingSettings.Template ?? Templates.Body);
        }
        else
        {
            loggerConfiguration.MinimumLevel.Is(loggingSettings.LogLevel);
        }

        ConfigureOverrides(loggerConfiguration);
        WithProperties(loggerConfiguration, loggingSettings);
        WithOverrides(loggerConfiguration, loggingSettings);

        Configure(loggerConfiguration, loggingSettings, loggingSettings.Email);
        Configure(loggerConfiguration, loggingSettings, loggingSettings.File);
        Configure(loggerConfiguration, loggingSettings.Seq);
        Configure(loggerConfiguration, loggingSettings.GrayLog);
    }

    private static void WithProperties(LoggerConfiguration loggerConfiguration, LoggingSettings loggingSettings)
    {
        foreach (var property in loggingSettings.Properties)
        {
            loggerConfiguration.Enrich.WithProperty(property.Key, property.Value);
        }
    }

    private static void WithOverrides(LoggerConfiguration loggerConfiguration, LoggingSettings loggingSettings)
    {
        foreach (var property in loggingSettings.Overrides)
        {
            loggerConfiguration.MinimumLevel.Override(property.Key, property.Value);
        }
    }

    private static void ConfigureOverrides(LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)

            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Model.Validation", LogEventLevel.Warning)

            .MinimumLevel.Override("MicroElements.Swashbuckle.FluentValidation", LogEventLevel.Warning)
            .MinimumLevel.Override("Ixia.Api.Auth.Basic.Simple.Handler.BasicAuthorizationHandler", LogEventLevel.Warning)
            ;
    }

    private static void Configure(
        LoggerConfiguration loggerConfiguration,
        LoggingSettings loggingSettings,
        EmailLoggingConfiguration configureLogToEmail)
    {
        if (configureLogToEmail != null)
        {
            loggerConfiguration.WriteTo.Email(new EmailSinkOptions
            {
                From = configureLogToEmail.From,
                To = configureLogToEmail.To.ToList(),
                Host = configureLogToEmail.Host,
                Port = configureLogToEmail.Port,
                IsBodyHtml = false,
                Subject = new Serilog.Formatting.Display.MessageTemplateTextFormatter(configureLogToEmail.Template ?? Templates.Subject),
                Body = new Serilog.Formatting.Display.MessageTemplateTextFormatter(
                    configureLogToEmail.Template ?? loggingSettings.Template ?? Templates.Body),
            }, new BatchingOptions
            {
                EagerlyEmitFirstEvent = true,
                BatchSizeLimit = 1000,
                QueueLimit = 50000,
                BufferingTimeLimit = TimeSpan.FromSeconds(5)
            },
            restrictedToMinimumLevel: configureLogToEmail.LogLevel);
        }
    }

    private static void Configure(LoggerConfiguration loggerConfiguration,
        LoggingSettings loggingSettings, FileLoggingConfiguration fileLogConfiguration)
    {
        if (fileLogConfiguration != null)
        {
            loggerConfiguration.WriteTo.File(
                path: fileLogConfiguration.Path,
                restrictedToMinimumLevel: fileLogConfiguration.LogLevel,
                outputTemplate: fileLogConfiguration.Template ?? loggingSettings.Template ?? Templates.Body,
                rollingInterval: RollingInterval.Day);
        }
    }

    private static void Configure(LoggerConfiguration loggerConfiguration, SeqLoggingConfiguration seqLoggingConfiguration)
    {
        if (seqLoggingConfiguration != null)
        {
            loggerConfiguration.WriteTo.Seq(
                serverUrl: seqLoggingConfiguration.ServerUrl,
                apiKey: seqLoggingConfiguration.ApiKey,
                restrictedToMinimumLevel: seqLoggingConfiguration.LogLevel,
                batchPostingLimit: 1000,
                period: TimeSpan.FromSeconds(10));
        }
    }

    private static void Configure(LoggerConfiguration loggerConfiguration, GrayLogLoggingConfiguration grayLogLoggingConfiguration)
    {
        if (grayLogLoggingConfiguration != null)
        {
            loggerConfiguration.Enrich.WithProperty("Stream", grayLogLoggingConfiguration.Stream);

            loggerConfiguration.WriteTo.Graylog(new GraylogSinkOptions
            {
                HostnameOrAddress = grayLogLoggingConfiguration.Host,
                Port = grayLogLoggingConfiguration.Port,
                TransportType = Serilog.Sinks.Graylog.Core.Transport.TransportType.Udp,
                MinimumLogEventLevel = grayLogLoggingConfiguration.LogLevel
            });
        }
    }
}
