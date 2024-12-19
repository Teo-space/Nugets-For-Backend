using Api.Calls.LogTo.Console.Domain;

namespace Api.Calls.LogTo.Console.Interfaces;

public interface IApiCallsLogger
{
    public void Write(Log log);
}
