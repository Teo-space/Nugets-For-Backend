using Api.Distributed.Configuration.Interfaces;
using Api.Distributed.Configuration.Interfaces.Settings;
using dotnet_etcd;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api.Distributed.Configuration.BackgroundServices;

class UpdateConfigurationPeriodically
(
    DistributedConfigurationSettings Settings,
    ILogger<UpdateConfigurationPeriodically> Logger,
    IAppConfigurationSource ConfigurationSource
)
    : BackgroundService
{
    private string ServiceName => GetType().Name;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation("[{Settings}]", Settings);

        while (stoppingToken.IsCancellationRequested == false)
        {
            await Task.Delay(1000 * 10, stoppingToken);

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
