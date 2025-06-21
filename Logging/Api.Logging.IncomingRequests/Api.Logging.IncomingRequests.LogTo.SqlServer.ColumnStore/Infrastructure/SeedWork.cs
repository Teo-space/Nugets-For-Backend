using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Api.Logging.IncomingRequests.LogTo.SqlServer.ColumnStore.Infrastructure;

internal static class SeedWork
{
    public static void CreateTableIfNotExists(string connectionString, string tableName)
    {
        tableName = $"{tableName}_INCOMING_LOGS";

        string sql =
$@"
IF NOT EXISTS 
(
		SELECT TOP(1) 1 
		FROM sys.objects 
		WHERE object_id = OBJECT_ID(N'{tableName}') AND type in (N'U')
)
BEGIN

CREATE TABLE [dbo].[{tableName}]
(
	[DATE] [DateTime] NOT NULL,
	[SERVICE_NAME] varchar(300) NOT NULL,
	[REQUEST_ID] varchar(100) NOT NULL,
	[EXECUTE_TIME] float NOT NULL,
	[TRACE_ID] varchar(100) NOT NULL,

	[USER_ID] varchar(100) NOT NULL,
	[IDENTITY_NAME] varchar(100) NOT NULL,

	[REQUEST_SCHEME] varchar(20) NOT NULL,
	[REQUEST_METHOD] varchar(20) NOT NULL,
	[REQUEST_CONTENT_TYPE] varchar(100) NOT NULL,
	[REQUEST_HEADERS] varchar(max) NOT NULL,

	[REQUEST_HOST] varchar(1000) NOT NULL,
	[REQUEST_PATH] varchar(1000) NOT NULL,
	[REQUEST_QUERY] varchar(8000) NOT NULL,
	[REQUEST_BODY] varchar(max) NOT NULL,

	[RESPONSE_CODE] int NOT NULL,
	[RESPONSE_CONTENT_TYPE] varchar(100) NOT NULL,
	[RESPONSE_HEADERS] varchar(max) NOT NULL,
	[RESPONSE_BODY] varchar(max) NOT NULL,

	[ERROR_MESSAGE] varchar(1000) NOT NULL,

	INDEX {tableName}_COLUMNSTORE CLUSTERED COLUMNSTORE ORDER ([DATE])
);

END
";
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            db.Execute(sql);
        }
    }

}
