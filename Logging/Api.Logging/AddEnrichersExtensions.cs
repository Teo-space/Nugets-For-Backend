using Api.Logging.Enrichers;
using Serilog.Core;

public static class AddEnrichersExtensions
{
    public static IServiceCollection AddCorrelationEnricher(this IServiceCollection services)
    {
        services.ApiCorrelationService();
        services.AddHttpContextAccessor();
        services.AddTransient<ILogEventEnricher, CorrelationEnricher>();


        return services;
    }

    public static IServiceCollection AddHttpContextEnricher(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<ILogEventEnricher, HttpContextEnricher>();


        return services;
    }
}