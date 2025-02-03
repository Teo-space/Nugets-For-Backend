namespace Api.Logging.Settings;

public sealed record LoggingSettings
{
    public string Template { get; set; } = default;

    public EmailLoggingConfiguration Email { get; set; }
    public FileLoggingConfiguration File { get; set; }
    public SeqLoggingConfiguration Seq { get; set; }

    public SeqLoggingConfiguration GrayLog { get; set; }

    public LoggingSettings AddEmailLogging(string From, IReadOnlyCollection<string> To, string Host, int Port = 25, string Template = default)
    {
        this.Email = new EmailLoggingConfiguration(From, To, Host, Port, Template);

        return this;
    }

    public LoggingSettings AddFileLogging(string Path, string Template = default)
    {
        this.File = new FileLoggingConfiguration(Path, Template);

        return this;
    }

    public LoggingSettings AddSeqLogging(string ServerUrl, string ApiKey)
    {
        this.Seq = new SeqLoggingConfiguration(ServerUrl, ApiKey);

        return this;
    }
}
