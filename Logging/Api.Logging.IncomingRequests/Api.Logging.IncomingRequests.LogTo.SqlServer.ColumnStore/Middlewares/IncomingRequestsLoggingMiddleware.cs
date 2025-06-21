using System.Diagnostics;
using System.Text;
using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Domain;
using Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Middlewares;

public sealed record IncomingRequestsLoggingMiddleware(RequestDelegate NextDelegate,
    IIncomingRequestsLogger IncomingRequestsLogger,
    ILogger<IncomingRequestsLoggingMiddleware> Logger,
    string ServiceName)
{

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var log = new Log
        {
            ServiceName = ServiceName,
            Date = DateTime.Now,
            RequestId = Ulid.NewUlid().ToString(),
            TraceId = httpContext.TraceIdentifier
        };

        try
        {
            await FillRequestInfo(httpContext, log);
            //Вызов NextDelegate
            await FillResponseInfo(httpContext, log, NextDelegate);
            //Нужно заполнять после вызова NextDelegate
            FillUserInfo(httpContext, log);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, @"
* ServiceName: {serviceName}
* RequestId: {RequestId}
* UserId: {UserId}
* Scheme: {Scheme}
* Method: {Method}
* Path: {Path}
* Query: {Query}
* Body: {Body}",
ServiceName,
log.RequestId,
log.UserInfo.UserId,
log.RequestInfo.Scheme,
log.RequestInfo.Method,
log.RequestInfo.Path,
log.RequestInfo.Query,
log.RequestInfo.Body);
        }
        finally
        {
            IncomingRequestsLogger.Write(log);
        }
    }

    private async Task FillRequestInfo(HttpContext httpContext, Log log)
    {
        HttpRequest request = httpContext.Request;

        log.RequestInfo.Scheme = request.Scheme;
        log.RequestInfo.Method = request.Method;
        log.RequestInfo.ContentType = request.ContentType;
        log.RequestInfo.Headers = FormatHeaders(request.Headers);

        log.RequestInfo.Host = request.Host.ToString();
        log.RequestInfo.Path = request.Path;
        log.RequestInfo.Query = request.QueryString.ToString();
        log.RequestInfo.Body = ReadBodyFromForm(request) ?? await ReadBodyFromBody(request);
    }

    private async Task FillResponseInfo(HttpContext httpContext, Log log, RequestDelegate nextDelegate)
    {
        HttpResponse response = httpContext.Response;
        var originalResponseBody = response.Body;
        using var newResponseBody = new MemoryStream();
        response.Body = newResponseBody;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await nextDelegate(httpContext);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null && string.IsNullOrEmpty(ex.InnerException.Message) == false)
            {
                log.ErrorMessage = $@"{ex.Message}
InnerException: {ex.InnerException.Message}";
            }
            else
            {
                log.ErrorMessage = ex.Message;
            }

            httpContext.Response.StatusCode = 500;

            Logger.LogError(ex, @"
* ServiceName: {serviceName}
* RequestId: {RequestId}
* UserId: {UserId}
* Scheme: {Scheme}
* Method: {Method}
* Path: {Path}
* Query: {Query}
* Body: {Body}",
ServiceName,
log.RequestId,
log.UserInfo.UserId,
log.RequestInfo.Scheme,
log.RequestInfo.Method,
log.RequestInfo.Path,
log.RequestInfo.Query,
log.RequestInfo.Body);
        }

        stopwatch.Stop();
        log.ExecuteTime = (float)stopwatch.Elapsed.TotalSeconds;

        newResponseBody.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(response.Body).ReadToEndAsync();
        newResponseBody.Seek(0, SeekOrigin.Begin);
        await newResponseBody.CopyToAsync(originalResponseBody);

        log.ResponseInfo.StatusCode = response.StatusCode;
        log.ResponseInfo.ContentType = response.ContentType;
        log.ResponseInfo.Headers = FormatHeaders(response.Headers);
        log.ResponseInfo.Body = responseBodyText[..Math.Min(responseBodyText.Length, 3000)];

        var contextFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (contextFeature != null && contextFeature.Error != null)
        {
            log.ErrorMessage = contextFeature.Error.Message;
        }
    }

    private void FillUserInfo(HttpContext httpContext, Log log)
    {
        log.UserInfo.UserId = httpContext.User?.FindFirst("userId")?.Value ?? "";
        log.UserInfo.IdentityName = httpContext.User?.Identity?.Name ?? "";
    }

    private static string ReadBodyFromForm(HttpRequest request)
    {
        try
        {
            if (request.HasFormContentType && request.Form != null && request.Form.Any())
            {
                var stringBuilder = new StringBuilder();

                foreach (var item in request.Form)
                {
                    stringBuilder.Append($"[{item.Key}:{item.Value.ToString()}]");
                }

                return stringBuilder.ToString();
            }
        }
        catch
        {
            //Ignore
        }

        return string.Empty;
    }

    private static string FormatHeaders(IHeaderDictionary headers)
    {
        var stringBuilder = new StringBuilder();

        foreach (var item in headers)
        {
            stringBuilder.Append($"[{item.Key}:{item.Value.ToString()}]");
        }

        return stringBuilder.ToString();
    }

    private static async Task<string> ReadBodyFromBody(HttpRequest request)
    {
        request.EnableBuffering();
        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = await streamReader.ReadToEndAsync();
        request.Body.Position = 0;
        return requestBody[..Math.Min(requestBody.Length, 5000)];
    }
}