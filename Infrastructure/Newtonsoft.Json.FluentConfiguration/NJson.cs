namespace Newtonsoft.Json.FluentConfiguration;

public static class NJson
{
    public static string Serialize<TClass>(this TClass o) where TClass : class
    {
        return JsonConvert.SerializeObject(o, FluentJsonSettings.JsonSerializerSettings);
    }

    public static TClass Serialize<TClass>(string json) where TClass : class
    {
        return JsonConvert.DeserializeObject<TClass>(json, FluentJsonSettings.JsonSerializerSettings);
    }
}
