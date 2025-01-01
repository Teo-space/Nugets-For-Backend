using System.Collections.Concurrent;
using Api.Calls.LogTo.SqlServer.ColumnStore.Interfaces;
using Api.Calls.LogTo.SqlServer.ColumnStore.Domain;

namespace Api.Calls.LogTo.SqlServer.ColumnStore.Services;

internal class ApiCallsLogger : IApiCallsLogger
{
    internal static readonly ConcurrentBag<Log> Logs = new ConcurrentBag<Log>();

    public void Write(Log log) => Logs.Add(log);
}
