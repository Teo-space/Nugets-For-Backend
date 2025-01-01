using Api.Requests.LogTo.ClickHouse.Domain;


namespace Api.Requests.LogTo.ClickHouse.Interfaces;

public interface IApiRequestsLogger
{
    public void Write(Log log);
}
