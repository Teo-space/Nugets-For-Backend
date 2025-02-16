namespace Api.Distributed.Configuration.Interfaces.Settings;

public record DistributedConfigurationSettings
{
    /// <summary>
    /// Группа конфигов
    /// </summary>
    public string Group { get; init; }

    /// <summary>
    /// Путь диапазона ключей конфигурации
    /// </summary>
    public string RangePath => $"Configuration/{Group}/";

    /// <summary>
    /// Строка подключения к ETCD
    /// </summary>
    public string Connection { get; init; }

    /// <summary>
    /// Интервал в секундах для чтения конфигов из Etcd
    /// </summary>
    public uint Interval { get; init; } = 10;

    public Authentication Authentication { get; init; }
}
