using Api.Date;
using Microsoft.Extensions.DependencyInjection;

public static class ApiDateDependencyInjection
{
    public static void AddDateService(this IServiceCollection services) => services.AddScoped<IDateService, DateService>();

}
