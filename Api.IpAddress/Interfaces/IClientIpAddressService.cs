namespace Api.IpAddress.Interfaces;


/// <summary>
/// Сервис для получения IP адреса клиента
/// </summary>
public interface IClientIpAddressService
{
    /// <summary>
    /// Получить IP адрес клиента
    /// </summary>
    public string GetClientIpAddress();
}
