using Api.Logging.Enrichers;
using Serilog.Core;

namespace Api.Logging;

public static class AddEnrichersExt
{
    public static IServiceCollection AddHttpContextEnricher(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<ILogEventEnricher, HttpContextEnricher>();

        return services;
    }
}
