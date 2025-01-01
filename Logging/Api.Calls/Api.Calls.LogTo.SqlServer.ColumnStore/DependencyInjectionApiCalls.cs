using Api.Calls.LogTo.SqlServer.ColumnStore.BackgroundServices;
using Api.Calls.LogTo.SqlServer.ColumnStore.Infrastructure;
using Api.Calls.LogTo.SqlServer.ColumnStore.Interfaces;
using Api.Calls.LogTo.SqlServer.ColumnStore.Middlewares;
using Api.Calls.LogTo.SqlServer.ColumnStore.Services;
using Api.Calls.LogTo.SqlServer.ColumnStore.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Calls.LogTo.SqlServer.ColumnStore;

public static class DependencyInjectionApiCalls
{
    public static IServiceCollection AddApiCallsLogger(this IServiceCollection services,
        Action<ApiCallsSettings> configure)
    {
        {
            ApiCallsSettings settings = new ApiCallsSettings();
            configure(settings);
            SeedWork.CreateTableIfNotExists(settings.ConnectionString, settings.TableName);
        }

        services.AddSingleton<IApiCallsLogger, ApiCallsLogger>();

        services.AddHostedService((serviceProvider) =>
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
