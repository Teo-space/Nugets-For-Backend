namespace Api.Logging.Settings;

public sealed record EmailLoggingConfiguration(string From, IReadOnlyCollection<string> To, string Host, int Port = 25,
    string Template = default);

public sealed record FileLoggingConfiguration(string Path, string Template = default);

public sealed record SeqLoggingConfiguration(string ServerUrl, string ApiKey);