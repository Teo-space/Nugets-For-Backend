using Api.Logging.IncomingRequests.LogTo.Console.Interfaces;
using Api.Logging.IncomingRequests.LogTo.Console.Middlewares;
using Api.Logging.IncomingRequests.LogTo.Console.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Logging.IncomingRequests.LogTo.Console;

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
