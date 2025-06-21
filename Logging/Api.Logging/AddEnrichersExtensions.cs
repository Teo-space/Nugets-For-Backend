using Api.Logging.Enrichers;
using Serilog.Core;

public static class AddEnrichersExtensions
{

    public static IServiceCollection AddHttpContextEnricher(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<ILogEventEnricher, HttpContextEnricher>();


        return services;
    }
}