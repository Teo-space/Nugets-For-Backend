using Api.Logging.IncomingRequests.LogTo.Console.Domain;

namespace Api.Logging.IncomingRequests.LogTo.Console.Interfaces;

public interface IIncomingRequestsLogger
{
    public void Write(Log log);
}
