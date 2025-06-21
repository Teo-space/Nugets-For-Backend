using Api.Logging.OutgoingRequests.LogTo.ClickHouse.Domain;

namespace Api.Logging.OutgoingRequests.LogTo.ClickHouse.Interfaces;

public interface IOutgoingRequestsLogger
{
    public void Write(Log log);
}
