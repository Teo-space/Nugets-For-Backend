namespace Newtonsoft.Json.FluentConfiguration.Models;

public abstract class Rule
{
    private Dictionary<string, object> rule { get; } = new Dictionary<string, object>();

    protected void AddRule(string key, object value)
    {
        if (rule.ContainsKey(key))
        {
            rule.Add(key, value);
        }
        else
        {
            rule[key] = value;
        }
    }

    protected IEnumerable<KeyValuePair<string, object>> RegisteredRules
    {
        get
        {
            return rule.AsEnumerable();
        }
    }
}
