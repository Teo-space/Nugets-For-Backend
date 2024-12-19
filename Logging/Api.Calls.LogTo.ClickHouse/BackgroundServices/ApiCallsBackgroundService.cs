using Api.Calls.LogTo.ClickHouse.Domain;
using Api.Calls.LogTo.ClickHouse.Infrastructure;
using Api.Calls.LogTo.ClickHouse.Services;
using Api.Calls.LogTo.ClickHouse.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api.Calls.LogTo.ClickHouse.BackgroundServices;

internal class ApiCallsBackgroundService
(
    ApiCallsSettings Settings, 
    ILogger<ApiCallsBackgroundService> Logger
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
                ServiceName, interval, ApiCallsLogger.Logs.Count);

            if ((interval >= Settings.Interval && ApiCallsLogger.Logs.Count > 0
                || ApiCallsLogger.Logs.Count >= Settings.BatchSize) == false)
            {
                continue;
            }

            interval = 0;
            List<Log> logs = new List<Log>();

            while (ApiCallsLogger.Logs.TryTake(out Log log))
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
                Logger.LogError(ex, "[{ServiceName}] Необработанная ошибка при сохранении логов", ServiceName);

                retryCount += 1;
                if (retryCount >= 10 && logs.Count > 100_000)
                {
                    logs.Clear();
                    retryCount = 0;

                    continue;
                }

                foreach (Log log in logs)
                {
                    ApiCallsLogger.Logs.Add(log);
                }
            }
            finally
            {
                logs.Clear();
            }
        }

        if (ApiCallsLogger.Logs.Count > 0)
        {
            //Записать остатки логов при завершении сервиса
            try
            {
                await BulkLogWriter.WriteLogs(Settings.ConnectionString, Settings.TableName, ApiCallsLogger.Logs);

                Logger.LogDebug("[{ServiceName}] Сохранено логов: {Logs}",
                    ServiceName, ApiCallsLogger.Logs.Count);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "[{ServiceName}] Необработанная ошибка при сохранении логов", ServiceName);
            }
        }
    }
}

