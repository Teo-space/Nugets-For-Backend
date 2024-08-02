namespace Api.IpAddress.Interfaces;

/// <summary>
/// Сервис для получения IP адреса сервера
/// </summary>
public interface IServerIpAddressService
{
    /// <summary>
    /// получить IP адрес сервера
    /// </summary>
    public string GetServerIpAddress();
}
