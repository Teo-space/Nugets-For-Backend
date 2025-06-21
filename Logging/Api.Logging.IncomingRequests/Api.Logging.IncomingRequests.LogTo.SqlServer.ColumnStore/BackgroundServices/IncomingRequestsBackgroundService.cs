using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Domain;
using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Infrastructure;
using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Services;
using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.BackgroundServices;

internal class IncomingRequestsBackgroundService
(
    IncomingRequestsLoggingSettings Settings,
    ILogger<IncomingRequestsBackgroundService> Logger
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

            if ((interval >= Settings.Interval && IncomingRequestsLogger.Logs.Count > 0
                || IncomingRequestsLogger.Logs.Count >= Settings.BatchSize) == false)
            {
                continue;
            }

            interval = 0;
            List<Log> logs = new List<Log>();

            while (IncomingRequestsLogger.Logs.TryTake(out Log log))
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
                    IncomingRequestsLogger.Logs.Add(log);
                }
            }
            finally
            {
                logs.Clear();
            }
        }

        if (IncomingRequestsLogger.Logs.Count > 0)
        {
            //Записать остатки логов при завершении сервиса
            try
            {
                await BulkLogWriter.WriteLogs(Settings.ConnectionString, Settings.TableName, IncomingRequestsLogger.Logs);

                Logger.LogDebug("[{ServiceName}] Сохранено логов: {Logs}",
                    ServiceName, IncomingRequestsLogger.Logs.Count);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "[{ServiceName}] Необработанная ошибка при сохранении логов", ServiceName);
            }
        }
    }
}

