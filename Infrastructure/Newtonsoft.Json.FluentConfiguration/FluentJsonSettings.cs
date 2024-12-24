using System.Reflection;
using Newtonsoft.Json.FluentConfiguration.ContractResolvers;

namespace Newtonsoft.Json.FluentConfiguration;

public static class FluentJsonSettings
{
    public static void AddConfigurations(params Assembly[] assemblies)
    {
        FluentContractResolver.SearchAssemblies(assemblies);
    }

    public static void Clear() => FluentContractResolver.Clear();

    public static JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        MaxDepth = 10,
        ContractResolver = new FluentContractResolver()
    };
}
