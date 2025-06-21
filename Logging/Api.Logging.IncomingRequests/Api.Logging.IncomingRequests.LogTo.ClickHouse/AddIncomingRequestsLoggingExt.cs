using Api.Logging.IncomingRequests.LogTo.ClickHouse.BackgroundServices;
using Api.Logging.IncomingRequests.LogTo.ClickHouse.Infrastructure;
using Api.Logging.IncomingRequests.LogTo.ClickHouse.Interfaces;
using Api.Logging.IncomingRequests.LogTo.ClickHouse.Middlewares;
using Api.Logging.IncomingRequests.LogTo.ClickHouse.Services;
using Api.Logging.IncomingRequests.LogTo.ClickHouse.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Logging.IncomingRequests.LogTo.ClickHouse;

public static class AddIncomingRequestsLoggingExt
{
    public static IServiceCollection AddApiCallsLogger(this IServiceCollection services,
        Action<IncomingRequestsLoggingSettings> configure)
    {
        {
            IncomingRequestsLoggingSettings settings = new IncomingRequestsLoggingSettings();

            configure(settings);

            SeedWork.CreateTableIfNotExists(settings.ConnectionString, settings.TableName).GetAwaiter().GetResult();
        }

        services.AddSingleton<IIncomingRequestsLogger, IncomingRequestsLogger>();

        services.AddHostedService((serviceProvider) =>
        {
            ILogger<IncomingRequestsBackgroundService> logger = serviceProvider.GetService<ILogger<IncomingRequestsBackgroundService>>();

            IncomingRequestsLoggingSettings settings = new IncomingRequestsLoggingSettings();

            configure(settings);

            return new IncomingRequestsBackgroundService(settings, logger);
        });

        return services;
    }

    public static IApplicationBuilder UseApiCallsLogger(this IApplicationBuilder app, string serviceName)
    {
        app.UseMiddleware<IncomingRequestsLoggingMiddleware>(serviceName);

        return app;
    }
}
