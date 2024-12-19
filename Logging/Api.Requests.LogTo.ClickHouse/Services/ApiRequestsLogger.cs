using Api.Requests.LogTo.ClickHouse.Interfaces;
using Api.Requests.LogTo.ClickHouse.Domain;
using System.Collections.Concurrent;

namespace Api.Requests.LogTo.ClickHouse.Services;

internal class ApiRequestsLogger : IApiRequestsLogger
{
    internal static readonly ConcurrentBag<Log> Logs = new ConcurrentBag<Log>();

    public void Write(Log log) => Logs.Add(log);
}
