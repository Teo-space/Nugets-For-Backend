using Api.Correlation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog.Events;

namespace Api.Logging.Enrichers;

internal class CorrelationEnricher(IHttpContextAccessor contextAccessor) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        ILogger<CorrelationEnricher> logger = contextAccessor?.HttpContext?.RequestServices?.GetService<ILogger<CorrelationEnricher>>();
        try
        {
            ICorrelationService correlationService = contextAccessor?.HttpContext?.RequestServices?.GetService<ICorrelationService>();
            if (correlationService != null)
            {
                string correlationId = correlationService.AsString;
                logger?.LogDebug($"correlationId: '{correlationId}'");

                if (correlationId != default)
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CorrelationId", correlationId));
                }
            }
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, ex.Message);
        }
    }
}
