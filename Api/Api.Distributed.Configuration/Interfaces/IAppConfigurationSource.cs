namespace Api.Distributed.Configuration.Interfaces;

public interface IAppConfigurationSource
{
    void Set(string key, string value);
}
