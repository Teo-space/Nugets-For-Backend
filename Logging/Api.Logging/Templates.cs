namespace Api.Logging;

public static class Templates
{
    public static string Subject => "{Application} {Module} {Env} {Level}";
    public static string Body =>
@"{Timestamp:dd.MM.yyyy HH:mm:ss.fff} [{Level:u3}] ({Application}/{Env}/{MachineName}/{Module}/{SourceContext})
{Message:lj}
{Exception}";
}
