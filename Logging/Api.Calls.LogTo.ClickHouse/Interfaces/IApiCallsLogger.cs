using Api.Calls.LogTo.ClickHouse.Domain;

namespace Api.Calls.LogTo.ClickHouse.Interfaces;

public interface IApiCallsLogger
{
    public void Write(Log log);
}
