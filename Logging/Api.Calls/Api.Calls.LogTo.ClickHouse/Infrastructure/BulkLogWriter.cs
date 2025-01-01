using ClickHouse.Client.Copy;
using Api.Calls.LogTo.ClickHouse.Domain;

namespace Api.Calls.LogTo.ClickHouse.Infrastructure;

public static class BulkLogWriter
{
    public static async Task WriteLogs(string connectionString, string tableName,
        IReadOnlyCollection<Log> logs)/// SETTINGS async_insert=1, wait_for_async_insert=1 
    {
        if (logs.Count == 0) return;

        using var bulkCopy = new ClickHouseBulkCopy(connectionString)
        {
            DestinationTableName = $"{tableName}_API_REQUEST_LOGS",
            ColumnNames = new[]
            {
                "DATE", "SERVICE_NAME", 
                "REQUEST_ID", "EXECUTE_TIME", "TRACE_ID",

                "USER_ID", "IDENTITY_NAME",

                "REQUEST_SCHEME", "REQUEST_METHOD", "REQUEST_CONTENT_TYPE", "REQUEST_HEADERS",
                "REQUEST_HOST", "REQUEST_PATH", "REQUEST_QUERY", "REQUEST_BODY",

                "RESPONSE_CODE", "RESPONSE_CONTENT_TYPE", "RESPONSE_HEADERS", "RESPONSE_BODY",
                "ERROR_MESSAGE"
            },
            MaxDegreeOfParallelism = 8,
            BatchSize = 100_000
        };

        await bulkCopy.InitAsync();

        IReadOnlyCollection<object[]> values = logs
            .Select(x => new object[]
            {
                x.Date, x.ServiceName, 
                x.RequestId, x.ExecuteTime, x.TraceId,

                x.UserInfo.UserId, x.UserInfo.IdentityName,

                x.RequestInfo.Scheme, x.RequestInfo.Method, x.RequestInfo.ContentType, x.RequestInfo.Headers,
                x.RequestInfo.Host, x.RequestInfo.Path, x.RequestInfo.Query, x.RequestInfo.Body,

                x.ResponseInfo.StatusCode, x.ResponseInfo.ContentType, x.ResponseInfo.Headers, x.ResponseInfo.Body,
                x.ErrorMessage
            }).ToArray();

        await bulkCopy.WriteToServerAsync(values);
    }

}
