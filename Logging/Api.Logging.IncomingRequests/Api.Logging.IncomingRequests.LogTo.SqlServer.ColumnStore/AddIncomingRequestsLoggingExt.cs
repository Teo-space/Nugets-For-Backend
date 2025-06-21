using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.BackgroundServices;
using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Infrastructure;
using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Interfaces;
using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Middlewares;
using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Services;
using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore;

public static class AddIncomingRequestsLoggingExt
{
    public static IServiceCollection AddApiCallsLogger(this IServiceCollection services,
        Action<IncomingRequestsLoggingSettings> configure)
    {
        {
            IncomingRequestsLoggingSettings settings = new IncomingRequestsLoggingSettings();

            configure(settings);

            SeedWork.CreateTableIfNotExists(settings.ConnectionString, settings.TableName);
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
