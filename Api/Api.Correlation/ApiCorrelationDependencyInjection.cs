using Api.Correlation;
using Microsoft.Extensions.DependencyInjection;

public static class ApiCorrelationDependencyInjection
{
    public static void AddDateService(this IServiceCollection services) => services.AddScoped<ICorrelationService, CorrelationService>();
}
