namespace Api.Distributed.Configuration.Interfaces.Settings;

public record Authentication
{
    public string Name { get; init; }
    public string Password { get; init; }
}
