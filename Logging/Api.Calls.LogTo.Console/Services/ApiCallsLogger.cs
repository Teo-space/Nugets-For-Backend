using Api.Calls.LogTo.Console.Domain;
using Api.Calls.LogTo.Console.Interfaces;

namespace Api.Calls.LogTo.Console.Services;

internal class ApiCallsLogger : IApiCallsLogger
{
    public void Write(Log log)
    {
        System.Console.WriteLine(
@$"
* Date :: {log.Date}
* ServiceName :: {log.ServiceName}

* RequestId :: {log.RequestId}
* ExecuteTime :: {log.ExecuteTime}
* TraceId :: {log.TraceId}

* ErrorMessage :: {log.ErrorMessage}

* Request.Scheme :: {log.RequestInfo.Scheme}
* Request.Method :: {log.RequestInfo.Method}
* Request.ContentType :: {log.RequestInfo.ContentType}
* Request.Headers :: {log.RequestInfo.Headers}

* Request.Host :: {log.RequestInfo.Host}
* Request.Path :: {log.RequestInfo.Path}
* Request.Query :: {log.RequestInfo.Query}
* Request.Body :: {log.RequestInfo.Body}

* Response.StatusCode :: {log.ResponseInfo.StatusCode}
* Response.Headers :: {log.ResponseInfo.Headers}
* Response.ContentType :: {log.ResponseInfo.ContentType}
* Response.Body :: {log.ResponseInfo.Body}
");
    }
}
