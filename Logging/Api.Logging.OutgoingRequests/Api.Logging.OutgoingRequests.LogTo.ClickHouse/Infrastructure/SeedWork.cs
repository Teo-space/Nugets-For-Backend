using ClickHouse.Client.ADO;
using ClickHouse.Client.Utility;


namespace Api.Logging.OutgoingRequests.LogTo.ClickHouse.Infrastructure;

public static class SeedWork
{
    public static async Task CreateTableIfNotExists(string connectionString, string tableName)
    {
        string sql =
$@"
CREATE TABLE IF NOT EXISTS {tableName}_OUTGOING_LOGS
(
    DATE DateTime('Europe/Moscow'), 
	SERVICE_NAME String, ACTION_NAME String, EXTERNAL_SERVICE_NAME String,
    REQUEST_ID String, EXECUTE_TIME Float32, TRACE_ID String, 

	INNER_ENTITY_ID String, OUTER_ENTITY_ID String,
	
    USER_ID String, IDENTITY_NAME String, 

	REQUEST_SCHEME String,
	REQUEST_METHOD String,
	REQUEST_CONTENT_TYPE String,
	REQUEST_HEADERS String,
	REQUEST_HOST String,
	REQUEST_PATH String,
	REQUEST_QUERY String,
	REQUEST_BODY String,
	
	RESPONSE_CODE Int32,
	RESPONSE_CONTENT_TYPE String,
	RESPONSE_HEADERS String,
	RESPONSE_BODY String,
	
	ERROR_MESSAGE String
)
ENGINE = MergeTree()
PARTITION BY toYYYYMM(DATE)
ORDER BY DATE
TTL DATE + INTERVAL 6 MONTH;
";
        using (var forCreateConnection = new ClickHouseConnection(connectionString))
        {
            await forCreateConnection.ExecuteStatementAsync(sql);
        }
    }

}
