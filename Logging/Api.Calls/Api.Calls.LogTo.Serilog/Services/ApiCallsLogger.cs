using Api.Calls.LogTo.Console.Domain;
using Api.Calls.LogTo.Console.Interfaces;
using Microsoft.Extensions.Logging;

namespace Api.Calls.LogTo.Console.Services;

internal class ApiCallsLogger(ILogger<ApiCallsLogger> logger) : IApiCallsLogger
{
    public void Write(Log log)
    {
        logger.LogInformation(
@"
* Date :: {Date}
* ServiceName :: {ServiceName}

* RequestId :: {RequestId}
* ExecuteTime :: {ExecuteTime}
* TraceId :: {log.TraceId}

* ErrorMessage :: {log.ErrorMessage}

* Request.Scheme :: {Scheme}
* Request.Method :: {Method}
* Request.ContentType :: {ContentType}
* Request.Headers :: {Headers}

* Request.Host :: {Host}
* Request.Path :: {Path}
* Request.Query :: {Query}
* Request.Body :: {Body}

* Response.StatusCode :: {StatusCode}
* Response.Headers :: {Headers}
* Response.ContentType :: {.ContentType}
* Response.Body :: {Body}",
log.Date,
log.ServiceName,

log.RequestId,
log.ExecuteTime,
log.TraceId,

log.ErrorMessage,

log.RequestInfo.Scheme,
log.RequestInfo.Method,
log.RequestInfo.ContentType,
log.RequestInfo.Headers,

log.RequestInfo.Host,
log.RequestInfo.Path,
log.RequestInfo.Query,
log.RequestInfo.Body,


log.ResponseInfo.StatusCode,
log.ResponseInfo.Headers,
log.ResponseInfo.ContentType,
log.ResponseInfo.Body);


    }
}
