using Api.Logging.IncomingRequests.LogTo.ILogger.Domain;

namespace Api.Logging.IncomingRequests.LogTo.ILogger.Interfaces;

public interface IIncomingRequestsLogger
{
    public void Write(Log log);
}
