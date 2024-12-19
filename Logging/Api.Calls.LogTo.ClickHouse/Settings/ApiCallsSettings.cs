namespace Api.Calls.LogTo.ClickHouse.Settings;

public class ApiCallsSettings
{
    /// <summary>
    /// Строка подключения к ClickHouse
    /// </summary>
    public string ConnectionString { get; set; }
    /// <summary>
    /// Имя таблицы для логов
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// Интервал в секундах для загрузки логов в БД
    /// </summary>
    public uint Interval { get; set; } = 5;
    /// <summary>
    /// Количество логов при котором произойдет загрузка в БД игнорируя Interval
    /// </summary>
    public uint BatchSize { get; set; } = 1000;

}
