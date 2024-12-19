using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog.Events;
using System.Security.Claims;

namespace Api.Logging.Enrichers;

class HttpContextEnricher(IHttpContextAccessor contextAccessor) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        ILogger<HttpContextEnricher> logger = contextAccessor?.HttpContext?.RequestServices?.GetService<ILogger<HttpContextEnricher>>();
        try
        {
            ClaimsPrincipal user = contextAccessor.HttpContext?.User;
            if(user == null) return;

            string userName = user?.Identity?.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                logger?.LogDebug($"UserName: '{userName}'");

                var userNameProperty = propertyFactory.CreateProperty("UserName", userName);
                logEvent.AddPropertyIfAbsent(userNameProperty);
            }

            string userId = user?.FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                logger?.LogDebug($"UserId: '{userId}'");

                var userNameProperty = propertyFactory.CreateProperty("UserId", userId);
                logEvent.AddPropertyIfAbsent(userNameProperty);
            }
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, ex.Message);
        }
    }
}