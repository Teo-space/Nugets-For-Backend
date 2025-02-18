using Api.Distributed.Configuration.Interfaces;
using Api.Distributed.Configuration.Interfaces.Settings;
using dotnet_etcd;
using Microsoft.Extensions.Configuration;

namespace Api.Distributed.Configuration.Services;

public class AppConfigurationSource(DistributedConfigurationSettings Settings)

    : ConfigurationProvider, IConfigurationSource, IAppConfigurationSource
{
    IConfigurationProvider IConfigurationSource.Build(IConfigurationBuilder builder) => this;

    public override void Set(string key, string value)
    {
        if (Data.TryGetValue(key, out string oldValue))
        {
            if (oldValue != value)
            {
                base.Set(key, value);
                OnReload();
            }
        }
        else
        {
            base.Set(key, value);
            OnReload();
        }
    }

    public override void Load()
    {
        Console.WriteLine("LOAD");

        using EtcdClient etcdClient = EtcdClientHelper.Create(Settings);

        IReadOnlyDictionary<string, string> groupConfigs = etcdClient.ReadRange(Settings);

        if (groupConfigs.Count > 0)
        {
            Data = groupConfigs.ToDictionary();
        }
    }
}