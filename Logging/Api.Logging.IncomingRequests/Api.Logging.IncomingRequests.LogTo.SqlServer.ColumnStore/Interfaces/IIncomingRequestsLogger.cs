using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Domain;

namespace Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Interfaces;

public interface IIncomingRequestsLogger
{
    public void Write(Log log);
}
