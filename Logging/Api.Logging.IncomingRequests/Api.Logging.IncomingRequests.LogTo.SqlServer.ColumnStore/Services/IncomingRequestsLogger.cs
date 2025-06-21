using System.Collections.Concurrent;
using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Domain;
using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Interfaces;

namespace Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Services;

internal class IncomingRequestsLogger : IIncomingRequestsLogger
{
    internal static readonly ConcurrentBag<Log> Logs = new ConcurrentBag<Log>();

    public void Write(Log log) => Logs.Add(log);
}
