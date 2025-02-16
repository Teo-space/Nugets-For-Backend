namespace Api.Logging;

public static class Templates
{
    public static string Subject => "{Application} {Module} {Level} {SourceContext}";
    public static string Body =>
@"[{Timestamp:dd.MM.yyyy HH:mm:ss.fff}] [{Level:u3}] [{CorrelationId}] ({Application}/{Environment}/{MachineName})
[{SourceContext}]
{Message:lj}
{Exception}";
}
