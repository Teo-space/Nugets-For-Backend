using Serilog;
using Serilog.Configuration;

namespace Api.Logging.Enrichers.Extensions;

public static class EnrichEnvironmentExt
{
    public static LoggerConfiguration WithEnv(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        return enrichmentConfiguration.WithProperty("Env", CheckEnvironment.IsDevelopment ? "PROD" : "DEV");
    }
}
