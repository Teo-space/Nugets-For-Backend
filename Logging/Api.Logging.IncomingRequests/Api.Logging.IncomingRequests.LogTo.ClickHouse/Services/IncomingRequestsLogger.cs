using System.Collections.Concurrent;
using Api.Logging.IncomingRequests.LogTo.ClickHouse.Domain;
using Api.Logging.IncomingRequests.LogTo.ClickHouse.Interfaces;

namespace Api.Logging.IncomingRequests.LogTo.ClickHouse.Services;

internal class IncomingRequestsLogger : IIncomingRequestsLogger
{
    internal static readonly ConcurrentBag<Log> Logs = new ConcurrentBag<Log>();

    public void Write(Log log) => Logs.Add(log);
}
