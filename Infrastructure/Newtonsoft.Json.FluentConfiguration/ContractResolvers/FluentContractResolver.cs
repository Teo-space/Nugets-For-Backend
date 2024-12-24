using Newtonsoft.Json.FluentConfiguration.Interfaces;
using Newtonsoft.Json.FluentConfiguration.Models;
using Newtonsoft.Json.FluentConfiguration.Settings;
using System.Reflection;

namespace Newtonsoft.Json.FluentConfiguration.ContractResolvers;

public class FluentContractResolver : DefaultContractResolver
{
    static List<ISerializationSettings> settings;

    public static void SearchAssemblies(params Assembly[] assemblies)
    {
        settings = assemblies
            .SelectMany(x => x
            .GetTypes())
            .Where(t => IsSubclassOfRawGeneric(t, typeof(SerializationSettings<>)))
            .Select(t => (ISerializationSettings)Activator.CreateInstance(t))
            .ToList();
    }

    static bool IsSubclassOfRawGeneric(Type toCheck, Type generic)
    {
        if (toCheck != generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.GetTypeInfo().IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.GetTypeInfo().BaseType;
            }
        }
        return false;
    }

    public static void Clear() => settings.Clear();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var contract = base.CreateProperty(member, memberSerialization);

        PropertyRule rule = settings
            .Where(x => x.GetType().GetTypeInfo().BaseType.GenericTypeArguments[0] == member.DeclaringType)
            .SelectMany(x => x.Rules
            .Select(r => r as PropertyRule)
            .Where(r => r != null && r.PropertyInfo.Name == member.Name))
            .FirstOrDefault();

        if (rule != null)
        {
            rule.Update(contract);
        }
        return contract;
    }
}
