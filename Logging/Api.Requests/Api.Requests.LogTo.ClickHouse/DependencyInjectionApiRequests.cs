using Api.Requests.LogTo.ClickHouse.BackgroundServices;
using Api.Requests.LogTo.ClickHouse.Infrastructure;
using Api.Requests.LogTo.ClickHouse.Interfaces;
using Api.Requests.LogTo.ClickHouse.Services;
using Api.Requests.LogTo.ClickHouse.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Requests.LogTo.ClickHouse;

public static class DependencyInjectionApiRequests
{
    public static IServiceCollection AddApiRequestsLogger(this IServiceCollection services,
        Action<ApiRequestsSettings> configure)
    {
        {
            ApiRequestsSettings settings = new ApiRequestsSettings();
            configure(settings);
            SeedWork.CreateTableIfNotExists(settings.ConnectionString, settings.TableName).GetAwaiter().GetResult();
        }

        services.AddSingleton<IApiRequestsLogger, ApiRequestsLogger>();

        services.AddHostedService((serviceProvider) =>
        {
            ILogger<ApiRequestsBackgroundService> logger = serviceProvider.GetService<ILogger<ApiRequestsBackgroundService>>();

            ApiRequestsSettings settings = new ApiRequestsSettings();
            configure(settings);

            return new ApiRequestsBackgroundService(settings, logger);
        });

        return services;
    }

}
