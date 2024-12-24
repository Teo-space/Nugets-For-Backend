using System.Reflection;

namespace Newtonsoft.Json.FluentConfiguration.Models;

public abstract class PropertyRule : Rule
{
    public MemberInfo PropertyInfo { get; protected set; }

    public void Update(JsonProperty contract)
    {
        var props = typeof(JsonProperty).GetProperties();
        foreach (var rule in RegisteredRules)
        {
            var property = props.Where(x => x.Name == rule.Key).FirstOrDefault();
            if (property != null)
            {
                var value = rule.Value;
                if (property.PropertyType == value.GetType())
                {
                    property.SetValue(contract, value);
                }
            }
        }
    }
}
