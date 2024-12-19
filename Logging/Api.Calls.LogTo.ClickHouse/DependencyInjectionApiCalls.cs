using Api.Calls.LogTo.ClickHouse.BackgroundServices;
using Api.Calls.LogTo.ClickHouse.Infrastructure;
using Api.Calls.LogTo.ClickHouse.Interfaces;
using Api.Calls.LogTo.ClickHouse.Middlewares;
using Api.Calls.LogTo.ClickHouse.Services;
using Api.Calls.LogTo.ClickHouse.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Calls.LogTo.ClickHouse;

public static class DependencyInjectionApiCalls
{
    public static IServiceCollection AddApiCallsLogger(this IServiceCollection services,
        Action<ApiCallsSettings> configure)
    {
        {
            ApiCallsSettings settings = new ApiCallsSettings();
            configure(settings);
            SeedWork.CreateTableIfNotExists(settings.ConnectionString, settings.TableName).GetAwaiter().GetResult();
        }

        services.AddSingleton<IApiCallsLogger, ApiCallsLogger>();

        services.AddHostedService((IServiceProvider serviceProvider) =>
        {
            ILogger<ApiCallsBackgroundService> logger = serviceProvider.GetService<ILogger<ApiCallsBackgroundService>>();

            ApiCallsSettings settings = new ApiCallsSettings();
            configure(settings);

            return new ApiCallsBackgroundService(settings, logger);
        });

        return services;
    }

    public static IApplicationBuilder UseApiCallsLogger(this IApplicationBuilder app, string serviceName)
    {
        app.UseMiddleware<ApiCallsLoggerMiddleware>(serviceName);
        return app;
    }
}
