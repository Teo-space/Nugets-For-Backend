using Api.Logging.IncomingRequests.LogTo.ClickHouse.Domain;

namespace Api.Logging.IncomingRequests.LogTo.ClickHouse.Interfaces;

public interface IIncomingRequestsLogger
{
    public void Write(Log log);
}
