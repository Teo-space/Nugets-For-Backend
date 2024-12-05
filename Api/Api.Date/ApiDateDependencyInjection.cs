using Microsoft.Extensions.DependencyInjection;

namespace Api.Date;

public static class ApiDateDependencyInjection
{
    public static void AddDateService(this IServiceCollection services) => services.AddScoped<IDateService, DateService>();

}
