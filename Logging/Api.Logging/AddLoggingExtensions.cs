using Api.Logging;
using Api.Logging.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Sinks.Email;
using Serilog.Sinks.SystemConsole.Themes;

public static class AddLoggingExtensions
{
    /// <summary>
    /// builder.Logging.AddSerilogLogging(builder.Services, builder.Configuration, "ServiceName");
    /// builder.Logging.AddSerilogLogging(builder.Services, builder.Configuration, "ServiceName", options =>
    ///{
    ///    options.AddEmailLogging("from@gmail.ru", ["to@gmail.ru"], "email.server", 25);
    ///    options.AddFileLogging("C:\\log\\Service\\log.txt");
    ///    options.AddSeqLogging("http://your.seq.com:5341", "asdasdasdad");
    ///});
    /// </summary>
    public static ILoggingBuilder AddSerilogLogging(this ILoggingBuilder loggingBuilder,
        IServiceCollection services,
        IConfiguration configuration, string appName,
        Action<LoggingSettings> options = default)
    {
        services.AddOptions<LoggingSettings>().Bind(configuration.GetSection(nameof(LoggingSettings)));

        loggingBuilder.ClearProviders();

        LoggingSettings loggingSettings = configuration.GetSection(nameof(LoggingSettings))
            ?.Get<LoggingSettings>() ?? new LoggingSettings();

        if (options != null)
        {
            options(loggingSettings);
        }

        services.AddSerilog((IServiceProvider serviceProvider, LoggerConfiguration loggerConfiguration) =>
        {
            loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(serviceProvider)

            .Enrich.WithProperty("Application", appName)

            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithAssemblyName()

            .MinimumLevel.Information()
            .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("MicroElements.Swashbuckle.FluentValidation", Serilog.Events.LogEventLevel.Warning)

            .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing.EndpointMiddleware", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker", Serilog.Events.LogEventLevel.Warning)
            ;
            IWebHostEnvironment webHostEnvironment = serviceProvider.GetService<IWebHostEnvironment>();
            if (webHostEnvironment?.IsDevelopment() ?? false)
            {
                loggerConfiguration.WriteTo.Console(outputTemplate: Templates.Body, theme: AnsiConsoleTheme.Code);
            }

            Configure(loggerConfiguration, loggingSettings, loggingSettings.Email);
            Configure(loggerConfiguration, loggingSettings, loggingSettings.File);
            Configure(loggerConfiguration, loggingSettings, loggingSettings.Seq);
        });

        return loggingBuilder;
    }

    private static void Configure(
        LoggerConfiguration loggerConfiguration, LoggingSettings loggingSettings, EmailLoggingConfiguration configureLogToEmail)
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
                Subject = new Serilog.Formatting.Display.MessageTemplateTextFormatter(Templates.Subject),
                Body = new Serilog.Formatting.Display.MessageTemplateTextFormatter(
                    configureLogToEmail.Template ?? loggingSettings.Template ?? Templates.Body),
            }, new BatchingOptions
            {
                EagerlyEmitFirstEvent = true,
                BatchSizeLimit = 1000,
                QueueLimit = 50000,
                BufferingTimeLimit = TimeSpan.FromSeconds(5)
            }, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error);
        }
    }

    private static void Configure(
        LoggerConfiguration loggerConfiguration, LoggingSettings loggingSettings, FileLoggingConfiguration fileLogConfiguration)
    {
        if (fileLogConfiguration != null)
        {
            loggerConfiguration.WriteTo.File(path: fileLogConfiguration.Path,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: fileLogConfiguration.Template ?? loggingSettings.Template ?? Templates.Body,
                rollingInterval: RollingInterval.Day);
        }
    }

    private static void Configure(
        LoggerConfiguration loggerConfiguration, LoggingSettings loggingSettings, SeqLoggingConfiguration seqLoggingConfiguration)
    {
        if (seqLoggingConfiguration != null)
        {
            loggerConfiguration.WriteTo.Seq(serverUrl: seqLoggingConfiguration.ServerUrl,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                batchPostingLimit: 1000,
                period: TimeSpan.FromSeconds(5),
                apiKey: seqLoggingConfiguration.ApiKey);
        }
    }

}