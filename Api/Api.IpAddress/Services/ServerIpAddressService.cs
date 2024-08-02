using Api.IpAddress.Interfaces;

namespace Api.IpAddress.Services;

internal class ServerIpAddressService : IServerIpAddressService
{
    private string _serverIpAddress { get; init; }
    public string GetServerIpAddress() => _serverIpAddress;


    public static IServerIpAddressService Create(string serverIpAddress)
    {
        return new ServerIpAddressService
        {
            _serverIpAddress = serverIpAddress
        };
    }
}
