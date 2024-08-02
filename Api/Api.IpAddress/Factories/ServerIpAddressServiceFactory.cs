using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using Api.IpAddress.Interfaces;
using Api.IpAddress.Services;

namespace Api.IpAddress.Factories;

public static class ServerIpAddressServiceFactory
{
    public static IServerIpAddressService Create()
    {
        IPAddress localMachineIpAddress = GetLocalMachineIpAddressAsync()
            ?? throw new InvalidOperationException("Не удалось получить IP адрес компьютера.");

        return ServerIpAddressService.Create(localMachineIpAddress.ToString());
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static IPAddress GetLocalMachineIpAddressAsync()
    {
        string hostName = Dns.GetHostName();
        IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);

        return GetIpV4Address(ipAddresses)
            ?? GetIpV6Address(ipAddresses);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static IPAddress GetIpV4Address(IEnumerable<IPAddress> ipAddresses)
        {
            IEnumerable<IPAddress> ipV4Addresses = ipAddresses.Where(static ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetwork);

            return ipV4Addresses.FirstOrDefault(static ipV4Address => ipV4Address.GetAddressBytes() is [192, 168, ..])
                ?? ipV4Addresses.FirstOrDefault();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static IPAddress GetIpV6Address(IEnumerable<IPAddress> ipAddresses)
            => ipAddresses.FirstOrDefault(static ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetworkV6);
    }
}
