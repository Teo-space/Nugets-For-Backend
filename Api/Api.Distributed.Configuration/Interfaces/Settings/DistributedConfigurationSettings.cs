namespace Api.Distributed.Configuration.Interfaces.Settings;

//Configuration/Default/ConnectionStrings:LOGS
public record DistributedConfigurationSettings
{
    /// <summary>
    /// Группа конфигов
    /// </summary>
    public string Group { get; init; } = "Default";

    /// <summary>
    /// Путь диапазона ключей конфигурации
    /// </summary>
    public string RangePath => $"Configuration/{Group}/";

    /// <summary>
    /// Строка подключения к ETCD
    /// </summary>
    public string Connection { get; init; }

    public Authentication Authentication { get; init; }
}
