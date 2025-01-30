using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Api.OTel;

public static class OTelDependencyInjection
{
    public static IServiceCollection AddOTel(this IServiceCollection services, string serviceName, Uri endpoint = default)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(serviceName);
                resource.AddEnvironmentVariableDetector();
                //resource.AddAttributes();
            })
            .WithMetrics(metrics =>
            {
                metrics.AddRuntimeInstrumentation();
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddHttpClientInstrumentation();

                metrics.AddSqlClientInstrumentation();

                if (endpoint != default)
                {
                    metrics.AddOtlpExporter(options =>
                    {
                        options.Endpoint = endpoint;
                    });
                }
                else
                {
                    metrics.AddOtlpExporter();
                }
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();

                tracing.AddEntityFrameworkCoreInstrumentation();
                tracing.AddSqlClientInstrumentation();
#if DEBUG
                tracing.AddConsoleExporter();
#endif
                if (endpoint != default)
                {
                    tracing.AddOtlpExporter(options =>
                    {
                        options.Endpoint = endpoint;
                    });
                }
                else
                {
                    tracing.AddOtlpExporter();
                }
            })
            ;



        return services;
    }

    public static ILoggingBuilder AddOTel(this ILoggingBuilder loggingBuilder, Uri endpoint = default)
    {
        loggingBuilder.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;

            if (endpoint != default)
            {
                logging.AddOtlpExporter(options =>
                {
                    options.Endpoint = endpoint;
                });
            }
            else
            {
                logging.AddOtlpExporter();
            }
        });

        return loggingBuilder;
    }
}
