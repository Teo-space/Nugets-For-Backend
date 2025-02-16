using Api.Distributed.Configuration.Interfaces;
using Api.Distributed.Configuration.Interfaces.Settings;
using dotnet_etcd;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api.Distributed.Configuration.BackgroundServices;

record UpdateConfigurationPeriodically
(
    DistributedConfigurationSettings Settings,
    ILogger<UpdateConfigurationPeriodically> Logger,
    IAppConfigurationSource ConfigurationSource,
    IConfiguration Configuration
)
    : IHostedService
{
    private string ServiceName => GetType().Name;

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task StartAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation("[{Settings}]", Settings);

        while (stoppingToken.IsCancellationRequested == false)
        {
            //await Task.Delay((int)(1000 * Settings.Interval), stoppingToken);
            await Task.Delay(1000, stoppingToken);

            try
            {
                await ProcessAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "[{ServiceName}] Необработанная ошибка при загрузке конфигурации", ServiceName);
            }
        }
    }

    protected async Task ProcessAsync()
    {
        using EtcdClient etcdClient = EtcdClientHelper.Create(Settings);

        IReadOnlyDictionary<string, string> groupConfigs = await etcdClient.ReadRangeAsync(Settings);

        if (groupConfigs.Count > 0)
        {
            Logger.LogInformation($"[{ServiceName}] Обновились ключи");

            foreach (var keyValue in groupConfigs)
            {
                Logger.LogInformation($"[{ServiceName}] {keyValue.Key}");

                ConfigurationSource.Set(keyValue.Key, keyValue.Value);
            }
        }
    }
}
