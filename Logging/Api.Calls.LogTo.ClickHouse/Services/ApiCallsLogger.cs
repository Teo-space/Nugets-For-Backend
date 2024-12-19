using Api.Calls.LogTo.ClickHouse.Interfaces;
using Api.Calls.LogTo.ClickHouse.Domain;
using System.Collections.Concurrent;

namespace Api.Calls.LogTo.ClickHouse.Services;

internal class ApiCallsLogger : IApiCallsLogger
{
    internal static readonly ConcurrentBag<Log> Logs = new ConcurrentBag<Log>();

    public void Write(Log log) => Logs.Add(log);
}
