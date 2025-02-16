using Api.Distributed.Configuration.Interfaces.Settings;
using dotnet_etcd;
using Etcdserverpb;
using Grpc.Core;

internal static class EtcdClientHelper
{
    public static EtcdClient Create(DistributedConfigurationSettings settings)
    {
        if (settings.Authentication is not null)
        {
            return new EtcdClient(settings.Connection);
        }
        else
        {
            return new EtcdClient(settings.Connection, configureChannelOptions: (options =>
            {
                options.Credentials = ChannelCredentials.Insecure;
            }));
        }
    }

    public static string Authenticate(this EtcdClient etcdClient, DistributedConfigurationSettings settings)
    {
        if (settings.Authentication is not null)
        {
            AuthenticateResponse authenticateResponse = etcdClient.Authenticate(new AuthenticateRequest()
            {
                Name = settings.Authentication.Name,
                Password = settings.Authentication.Password,
            });

            if (string.IsNullOrEmpty(authenticateResponse.Token.Trim()))
            {
                throw new InvalidOperationException($@"Не удалось авторизоватся при помощи указанных данных авторизации
{settings.Authentication}");
            }

            return authenticateResponse.Token;
        }

        return default;
    }

    public static async Task<string> AuthenticateAsync(this EtcdClient etcdClient, DistributedConfigurationSettings settings)
    {
        if (settings.Authentication is not null)
        {
            AuthenticateResponse authenticateResponse = await etcdClient.AuthenticateAsync(new AuthenticateRequest()
            {
                Name = settings.Authentication.Name,
                Password = settings.Authentication.Password,
            });

            if (string.IsNullOrEmpty(authenticateResponse.Token.Trim()))
            {
                throw new InvalidOperationException($@"Не удалось авторизоватся при помощи указанных данных авторизации
{settings.Authentication}");
            }

            return authenticateResponse.Token;
        }

        return default;
    }

    public static IReadOnlyDictionary<string, string> ReadRange(this EtcdClient etcdClient, DistributedConfigurationSettings settings)
    {
        string authToken = etcdClient.Authenticate(settings);

        RangeResponse rangeResponse =
            string.IsNullOrEmpty(authToken)
            ? etcdClient.GetRange(settings.RangePath)
            : etcdClient.GetRange(settings.RangePath, new Metadata()
            {
                new Metadata.Entry("token", authToken)
            });

        if (rangeResponse.Count == 0)
        {
            return new Dictionary<string, string>();
        }

        return rangeResponse.Kvs
            .Select(kv => KeyValuePair.Create(kv.Key.ToStringUtf8(), kv.Value.ToStringUtf8()))
            .ToDictionary();
    }

    public static async Task<IReadOnlyDictionary<string, string>> ReadRangeAsync(this EtcdClient etcdClient,
        DistributedConfigurationSettings settings)
    {
        string authToken = await etcdClient.AuthenticateAsync(settings);

        RangeResponse rangeResponse =
            string.IsNullOrEmpty(authToken)
            ? await etcdClient.GetRangeAsync(settings.RangePath)
            : await etcdClient.GetRangeAsync(settings.RangePath, new Metadata()
            {
                new Metadata.Entry("token", authToken)
            });

        if (rangeResponse.Count == 0)
        {
            return new Dictionary<string, string>();
        }

        return rangeResponse.Kvs
            .Select(kv => KeyValuePair.Create(kv.Key.ToStringUtf8(), kv.Value.ToStringUtf8()))
            .ToDictionary();
    }

}
