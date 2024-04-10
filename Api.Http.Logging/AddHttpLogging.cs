using Api.Http.Logging.Logger;
using Api.Http.Logging.MiddleWare;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

public static class AddHttpLogging
{
    public static IApplicationBuilder UseForwardedForHeaders(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        return app;
    }
    /// <summary>
    /// Добавляет Middleware для логирования запросов.
    /// Требует Scoped IHttpLogger
    /// </summary>
    public static IApplicationBuilder UseRequestResponseLogger(this IApplicationBuilder app, string serviceName)
    {
        app.UseMiddleware<HttpLoggerMiddleware>(serviceName);
        return app;
    }

    public static IServiceCollection AddConsoleHttpLogger(this IServiceCollection services)
    {
        services.AddScoped<IHttpLogger, ConsoleHttpLogger>();
        return services;
    }
    public static IServiceCollection AddHttpLogger(this IServiceCollection services)
    {
        services.AddScoped<IHttpLogger, HttpLogger>();
        return services;
    }



}
