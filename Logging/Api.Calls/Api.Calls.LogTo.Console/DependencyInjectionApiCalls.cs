using Api.Calls.LogTo.Console.Interfaces;
using Api.Calls.LogTo.Console.Middlewares;
using Api.Calls.LogTo.Console.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Calls.LogTo.Console;

public static class DependencyInjectionApiCalls
{
    public static IServiceCollection AddApiCallsLogger(this IServiceCollection services)
    {
        services.AddSingleton<IApiCallsLogger, ApiCallsLogger>();

        return services;
    }

    public static IApplicationBuilder UseApiCallsLogger(this IApplicationBuilder app, string serviceName)
    {
        app.UseMiddleware<ApiCallsLoggerMiddleware>(serviceName);
        return app;
    }
}
