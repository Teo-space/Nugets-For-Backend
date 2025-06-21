using Api.Logging.OutgoingRequests.LogTo.ClickHouse.Domain;
using Api.Logging.OutgoingRequests.LogTo.ClickHouse.Infrastructure;
using Api.Logging.OutgoingRequests.LogTo.ClickHouse.Services;
using Api.Logging.OutgoingRequests.LogTo.ClickHouse.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api.Logging.OutgoingRequests.LogTo.ClickHouse.BackgroundServices;

internal class OutgoingRequestsBackgroundService
(
    OutgoingRequestsLoggingSettings Settings,
    ILogger<OutgoingRequestsBackgroundService> Logger
)
    : BackgroundService
{
    private string ServiceName => GetType().Name;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int interval = 0;
        int retryCount = 0;

        while (stoppingToken.IsCancellationRequested == false)
        {
            await Task.Delay(1000, stoppingToken);
            interval += 1;

            if ((interval >= Settings.Interval && OutgoingRequestsLogger.Logs.Count > 0
                || OutgoingRequestsLogger.Logs.Count >= Settings.BatchSize) == false)
            {
                continue;
            }

            interval = 0;

            List<Log> logs = new List<Log>();

            while (OutgoingRequestsLogger.Logs.TryTake(out Log log))
            {
                logs.Add(log);
            }

            try
            {
                await BulkLogWriter.WriteLogs(Settings.ConnectionString, Settings.TableName, logs);

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
                    OutgoingRequestsLogger.Logs.Add(log);
                }
            }
            finally
            {
                logs.Clear();
            }
        }

        if (OutgoingRequestsLogger.Logs.Count > 0)
        {
            //Записать остатки логов при завершении сервиса
            try
            {
                await BulkLogWriter.WriteLogs(Settings.ConnectionString, Settings.TableName, OutgoingRequestsLogger.Logs);

                Logger.LogDebug("[{ServiceName}] Сохранено логов: {Logs}",
                    ServiceName, OutgoingRequestsLogger.Logs.Count);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "[{ServiceName}] Необработанная ошибка", ServiceName);
            }
        }
    }
}