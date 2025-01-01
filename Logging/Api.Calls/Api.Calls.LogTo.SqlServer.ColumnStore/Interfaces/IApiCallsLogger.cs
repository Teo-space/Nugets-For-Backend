using Api.Calls.LogTo.SqlServer.ColumnStore.Domain;

namespace Api.Calls.LogTo.SqlServer.ColumnStore.Interfaces;

public interface IApiCallsLogger
{
    public void Write(Log log);
}
