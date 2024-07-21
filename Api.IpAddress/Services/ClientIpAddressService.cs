using Api.IpAddress.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Api.IpAddress.Services;

internal class ClientIpAddressService : IClientIpAddressService
{
    private readonly HttpContext _httpContext;
    private readonly IServerIpAddressService _ipInfoService;
    public ClientIpAddressService(IHttpContextAccessor httpContextAccessor, IServerIpAddressService ipInfoService)
    {
        _httpContext = httpContextAccessor?.HttpContext;
        _ipInfoService = ipInfoService;
    }

    public string GetClientIpAddress()
    {
        string ipAddress = string.Empty;

        if (_httpContext is not null && _httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            var forwardedIps = (ICollection<string>)_httpContext.Request.Headers["X-Forwarded-For"];

            foreach (var forwardedIp in forwardedIps)
            {
                if (string.IsNullOrEmpty(ipAddress))
                {
                    foreach (var ip in forwardedIp.Split(','))
                    {
                        if (ip.Contains("127.0.0.1") || ip.Contains("::1"))
                            continue;

                        ipAddress = ip;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(ipAddress))
                    break;
            }

            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = forwardedIps.FirstOrDefault() ?? string.Empty;
        }
        else
        {
            ipAddress = _httpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
            if (ipAddress.Contains("127.0.0.1") || ipAddress.Contains("::1"))
            {
                ipAddress = _ipInfoService.GetServerIpAddress();
            }
        }

        return ipAddress;
    }


}
