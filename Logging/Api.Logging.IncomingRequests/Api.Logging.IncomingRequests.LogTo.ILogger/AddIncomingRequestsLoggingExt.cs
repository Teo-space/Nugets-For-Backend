using Api.Logging.IncomingRequests.LogTo.ILogger.Interfaces;
using Api.Logging.IncomingRequests.LogTo.ILogger.Middlewares;
using Api.Logging.IncomingRequests.LogTo.ILogger.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Logging.IncomingRequests.LogTo.ILogger;

public static class AddIncomingRequestsLoggingExt
{
    public static IServiceCollection AddApiCallsLogger(this IServiceCollection services)
    {
        services.AddSingleton<IIncomingRequestsLogger, IncomingRequestsLogger>();

        return services;
    }

    public static IApplicationBuilder UseApiCallsLogger(this IApplicationBuilder app, string serviceName)
    {
        app.UseMiddleware<IncomingRequestsLoggingMiddleware>(serviceName);

        return app;
    }
}
