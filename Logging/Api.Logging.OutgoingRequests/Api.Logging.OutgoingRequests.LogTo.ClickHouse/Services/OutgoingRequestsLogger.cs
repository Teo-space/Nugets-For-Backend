using System.Collections.Concurrent;
using Api.Logging.OutgoingRequests.LogTo.ClickHouse.Domain;
using Api.Logging.OutgoingRequests.LogTo.ClickHouse.Interfaces;

namespace Api.Logging.OutgoingRequests.LogTo.ClickHouse.Services;

internal class OutgoingRequestsLogger : IOutgoingRequestsLogger
{
    internal static readonly ConcurrentBag<Log> Logs = new ConcurrentBag<Log>();

    public void Write(Log log) => Logs.Add(log);
}
