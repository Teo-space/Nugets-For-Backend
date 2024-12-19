using Api.Logging.Enrichers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

public static class AddLoggingExtentions
{
    public static ILoggingBuilder AddSerilogLogging(this ILoggingBuilder loggingBuilder,
        IServiceCollection services,
        IConfiguration configuration)
    {
        loggingBuilder.ClearProviders();

        services.AddSerilog((IServiceProvider serviceProvider, LoggerConfiguration loggerConfiguration) =>
        {
            loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(serviceProvider)
            .Enrich.FromLogContext()
            .WriteTo.Console();
        });

        return loggingBuilder;
    }

    public static IServiceCollection AddCorrelationEnricher(this IServiceCollection services)
    {
        services.ApiCorrelationService();
        services.AddHttpContextAccessor();
        services.AddTransient<ILogEventEnricher, CorrelationEnricher>();


        return services;
    }

    public static IServiceCollection AddHttpContextEnricher(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<ILogEventEnricher, HttpContextEnricher>();


        return services;
    }
}

