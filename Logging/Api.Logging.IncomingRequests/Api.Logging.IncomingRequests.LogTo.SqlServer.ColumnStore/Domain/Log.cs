namespace Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Domain;

public sealed record Log
{
    public DateTime Date { get; set; }

    public string ServiceName { get; set; }

    public string RequestId { get; set; } = string.Empty;
    public float ExecuteTime { get; set; }
    public string TraceId { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public LogUserInfo UserInfo { get; set; } = new LogUserInfo();
    public LogRequestInfo RequestInfo { get; set; } = new LogRequestInfo();
    public LogResponseInfo ResponseInfo { get; set; } = new LogResponseInfo();
}
