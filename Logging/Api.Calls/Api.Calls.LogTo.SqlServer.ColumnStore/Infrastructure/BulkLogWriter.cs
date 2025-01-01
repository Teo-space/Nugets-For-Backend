using Api.Calls.LogTo.SqlServer.ColumnStore.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Api.Calls.LogTo.SqlServer.ColumnStore.Infrastructure;

public static class BulkLogWriter
{
    public static async Task WriteLogs(string connectionString, string tableName,
        IReadOnlyCollection<Log> logs)
    {
        if (logs.Count == 0) return;

        tableName = $"{tableName}_API_REQUEST_LOGS";

        DataTable dt = ToDataTable(logs);

        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString))
        {
            sqlBulkCopy.DestinationTableName = $"[dbo].[{tableName}]";
            sqlBulkCopy.WriteToServer(dt);
        }
    }
    /*
        string cmdText = @$"
insert into dbo.[{tableName}] (
    [DATE], [SERVICE_NAME],
    [REQUEST_ID], [EXECUTE_TIME], [TRACE_ID],

    [USER_ID], [IDENTITY_NAME],

    [REQUEST_SCHEME], [REQUEST_METHOD], [REQUEST_CONTENT_TYPE], [REQUEST_HEADERS],
    [REQUEST_HOST], [REQUEST_PATH, [REQUEST_QUERY], [REQUEST_BODY],

    [RESPONSE_CODE], [RESPONSE_CONTENT_TYPE], [RESPONSE_HEADERS], [RESPONSE_BODY],
    [ERROR_MESSAGE]
)
select Id, FirstName, LastName, Street, City, State, PhoneNumber, EmailAddress
from @logs";
    */
    private static DataTable ToDataTable(IReadOnlyCollection<Log> logs)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("DATE", typeof(DateTime));
        dt.Columns.Add("SERVICE_NAME", typeof(string));
        dt.Columns.Add("REQUEST_ID", typeof(string));
        dt.Columns.Add("EXECUTE_TIME", typeof(float));
        dt.Columns.Add("TRACE_ID", typeof(string));

        dt.Columns.Add("USER_ID", typeof(string));
        dt.Columns.Add("IDENTITY_NAME", typeof(string));

        dt.Columns.Add("REQUEST_SCHEME", typeof(string));
        dt.Columns.Add("REQUEST_METHOD", typeof(string));
        dt.Columns.Add("REQUEST_CONTENT_TYPE", typeof(string));
        dt.Columns.Add("REQUEST_HEADERS", typeof(string));

        dt.Columns.Add("REQUEST_HOST", typeof(string));
        dt.Columns.Add("REQUEST_PATH", typeof(string));
        dt.Columns.Add("REQUEST_QUERY", typeof(string));
        dt.Columns.Add("REQUEST_BODY", typeof(string));

        dt.Columns.Add("RESPONSE_CODE", typeof(int));
        dt.Columns.Add("RESPONSE_CONTENT_TYPE", typeof(string));
        dt.Columns.Add("RESPONSE_HEADERS", typeof(string));
        dt.Columns.Add("RESPONSE_BODY", typeof(string));

        dt.Columns.Add("ERROR_MESSAGE", typeof(string));

        foreach (Log log in logs)
        {
            DataRow dataRow = dt.NewRow();

            dataRow["DATE"] = log.Date;
            dataRow["SERVICE_NAME"] = log.ServiceName;
            dataRow["REQUEST_ID"] = log.RequestId ?? "";
            dataRow["EXECUTE_TIME"] = log.ExecuteTime;
            dataRow["TRACE_ID"] = log.TraceId ?? "";

            dataRow["USER_ID"] = log.UserInfo.UserId ?? "";
            dataRow["IDENTITY_NAME"] = log.UserInfo.IdentityName ?? "";

            dataRow["REQUEST_SCHEME"] = log.RequestInfo.Scheme ?? "";
            dataRow["REQUEST_METHOD"] = log.RequestInfo.Method ?? "";
            dataRow["REQUEST_CONTENT_TYPE"] = log.RequestInfo.ContentType ?? "";
            dataRow["REQUEST_HEADERS"] = log.RequestInfo.Headers ?? "";

            dataRow["REQUEST_HOST"] = log.RequestInfo.Host ?? "";
            dataRow["REQUEST_PATH"] = log.RequestInfo.Path ?? "";
            dataRow["REQUEST_QUERY"] = log.RequestInfo.Query ?? "";
            dataRow["REQUEST_BODY"] = log.RequestInfo.Body ?? "";

            dataRow["RESPONSE_CODE"] = log.ResponseInfo.StatusCode;
            dataRow["RESPONSE_CONTENT_TYPE"] = log.ResponseInfo.ContentType ?? "";
            dataRow["RESPONSE_HEADERS"] = log.ResponseInfo.Headers ?? "";
            dataRow["RESPONSE_BODY"] = log.ResponseInfo.Body ?? "";

            dataRow["ERROR_MESSAGE"] = log.ErrorMessage ?? "";

            dt.Rows.Add(dataRow);
        }

        return dt;
    }
}
