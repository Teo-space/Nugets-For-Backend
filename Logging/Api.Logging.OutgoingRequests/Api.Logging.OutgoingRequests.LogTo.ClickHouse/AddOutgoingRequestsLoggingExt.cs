using Api.Logging.OutgoingRequests.LogTo.ClickHouse.BackgroundServices;
using Api.Logging.OutgoingRequests.LogTo.ClickHouse.Infrastructure;
using Api.Logging.OutgoingRequests.LogTo.ClickHouse.Interfaces;
using Api.Logging.OutgoingRequests.LogTo.ClickHouse.Services;
using Api.Logging.OutgoingRequests.LogTo.ClickHouse.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Logging.OutgoingRequests.LogTo.ClickHouse;

public static class AddOutgoingRequestsLoggingExt
{
    public static IServiceCollection AddApiRequestsLogger(this IServiceCollection services,
        Action<OutgoingRequestsLoggingSettings> configure)
    {
        {
            OutgoingRequestsLoggingSettings settings = new OutgoingRequestsLoggingSettings();
            configure(settings);
            SeedWork.CreateTableIfNotExists(settings.ConnectionString, settings.TableName).GetAwaiter().GetResult();
        }

        services.AddSingleton<IOutgoingRequestsLogger, OutgoingRequestsLogger>();

        services.AddHostedService((serviceProvider) =>
        {
            ILogger<OutgoingRequestsBackgroundService> logger = serviceProvider.GetService<ILogger<OutgoingRequestsBackgroundService>>();

            OutgoingRequestsLoggingSettings settings = new OutgoingRequestsLoggingSettings();

            configure(settings);

            return new OutgoingRequestsBackgroundService(settings, logger);
        });

        return services;
    }

}
