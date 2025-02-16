using Api.Distributed.Configuration.Interfaces;
using Api.Distributed.Configuration.Interfaces.Settings;
using dotnet_etcd;
using Etcdserverpb;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api.Distributed.Configuration.BackgroundServices;

class UpdateConfigurationWatcher
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

        string authToken = await etcdClient.AuthenticateAsync(Settings);

        if (string.IsNullOrEmpty(authToken))
        {
            await etcdClient.WatchRangeAsync(Settings.RangePath, ProcessWatchResponse);
        }
        else
        {
            await etcdClient.WatchRangeAsync(Settings.RangePath, ProcessWatchResponse, new Metadata()
            {
                new Metadata.Entry("token", authToken)
            });
        }
    }

    protected void ProcessWatchResponse(WatchResponse watchResponse)
    {
        try
        {
            Dictionary<string, string> eventsOnPut = watchResponse.Events
                .Where(e => e.Type == Mvccpb.Event.Types.EventType.Put)
                .Select(e => KeyValuePair.Create(e.Kv.Key.ToStringUtf8(), e.Kv.Value.ToStringUtf8()))
                .ToDictionary();

            if (eventsOnPut.Count > 0)
            {
                Logger.LogInformation($"[{ServiceName}] Обновились ключи");

                foreach (var keyValue in eventsOnPut)
                {
                    Logger.LogInformation($"[{ServiceName}] {keyValue.Key}");

                    ConfigurationSource.Set(keyValue.Key, keyValue.Value);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "[{ServiceName}] Ошибка в ProcessWatchResponse", ServiceName);
        }
    }
}
