using Api.Requests.LogTo.ClickHouse.Domain;
using Api.Requests.LogTo.ClickHouse.Infrastructure;
using Api.Requests.LogTo.ClickHouse.Services;
using Api.Requests.LogTo.ClickHouse.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api.Requests.LogTo.ClickHouse.BackgroundServices;

internal class ApiRequestsBackgroundService
(
    ApiRequestsSettings Settings, 
    ILogger<ApiRequestsBackgroundService> Logger
)
    : BackgroundService
{
    private string ServiceName => this.GetType().Name;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int interval = 0;
        int retryCount = 0;

        while (stoppingToken.IsCancellationRequested == false)
        {
            await Task.Delay(1000, stoppingToken);
            interval += 1;

            Logger.LogDebug("[{ServiceName}] Шаг цикла. interval: {interval} Logs: {Logs}",
                ServiceName, interval, ApiRequestsLogger.Logs.Count);

            if ((interval >= Settings.Interval && ApiRequestsLogger.Logs.Count > 0
                || ApiRequestsLogger.Logs.Count >= Settings.BatchSize) == false)
            {
                continue;
            }

            interval = 0;

            List<Log> logs = new List<Log>();

            while (ApiRequestsLogger.Logs.TryTake(out Log log))
            {
                logs.Add(log);
            }

            try
            {
                await BulkLogWriter.WriteLogs(Settings.ConnectionString, Settings.TableName, logs);

                Logger.LogDebug("[{ServiceName}] Сохранено логов: {Logs}",
                    ServiceName, logs.Count);

                retryCount = 0;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "[{ServiceName}] Необработанная ошибка. логов: {Logs}",
                    ServiceName, logs.Count);

                retryCount += 1;
                if (retryCount >= 10 && logs.Count > 100_000)
                {
                    logs.Clear();
                    retryCount = 0;

                    continue;
                }

                foreach (Log log in logs)
                {
                    ApiRequestsLogger.Logs.Add(log);
                }
            }
            finally
            {
                logs.Clear();
            }
        }

        if (ApiRequestsLogger.Logs.Count > 0)
        {
            //Записать остатки логов при завершении сервиса
            try
            {
                await BulkLogWriter.WriteLogs(Settings.ConnectionString, Settings.TableName, ApiRequestsLogger.Logs);

                Logger.LogDebug("[{ServiceName}] Сохранено логов: {Logs}",
                    ServiceName, ApiRequestsLogger.Logs.Count);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "[{ServiceName}] Необработанная ошибка", ServiceName);
            }
        }
    }
}