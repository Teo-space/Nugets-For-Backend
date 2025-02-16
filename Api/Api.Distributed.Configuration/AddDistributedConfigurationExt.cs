using Api.Distributed.Configuration.BackgroundServices;
using Api.Distributed.Configuration.Interfaces;
using Api.Distributed.Configuration.Interfaces.Settings;
using Api.Distributed.Configuration.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public static class AddDistributedConfigurationExt
{
    public static void AddDistributedConfiguration(this IHostApplicationBuilder applicationBuilder,
        Action<DistributedConfigurationSettings> configure = default)
    {
        applicationBuilder.Services.AddOptions<DistributedConfigurationSettings>()
            .Bind(applicationBuilder.Configuration.GetSection(nameof(DistributedConfigurationSettings)));

        DistributedConfigurationSettings Settings = applicationBuilder.Configuration
            .GetSection(nameof(DistributedConfigurationSettings))
           ?.Get<DistributedConfigurationSettings>()
           ?? new DistributedConfigurationSettings();

        if (configure != default)
        {
            configure(Settings);
        }

        {
            var сonfigurationSource = new AppConfigurationSource(Settings);

            applicationBuilder.Services.AddSingleton<IAppConfigurationSource>(сonfigurationSource);

            applicationBuilder.Configuration.Add(сonfigurationSource);
        }

        applicationBuilder.Services.AddHostedService((serviceProvider) =>
        {
            ILogger<UpdateConfigurationPeriodically> logger = serviceProvider.GetService<ILogger<UpdateConfigurationPeriodically>>();
            IAppConfigurationSource appConfigurationSource = serviceProvider.GetRequiredService<IAppConfigurationSource>();

            return new UpdateConfigurationPeriodically(Settings, logger, appConfigurationSource,
                applicationBuilder.Configuration);
        });
    }

}




