using Api.IpAddress.Factories;
using Api.IpAddress.Interfaces;
using Api.IpAddress.Services;
using Microsoft.Extensions.DependencyInjection;

public static class IpAddressExtensions
{
    /// <summary>
    /// Добавить сервисы для получения Ip адреса сервера и клиента 
    /// </summary>
    public static IServiceCollection AddIpAddressInfoServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        IServerIpAddressService serverIpAddressService = ServerIpAddressServiceFactory.Create();
        services.AddSingleton(serverIpAddressService);

        services.AddScoped<IClientIpAddressService, ClientIpAddressService>();

        return services;
    }
}
