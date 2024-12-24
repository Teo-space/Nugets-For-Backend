using System.Linq.Expressions;
using Newtonsoft.Json.FluentConfiguration.Interfaces;
using Newtonsoft.Json.FluentConfiguration.Models;

namespace Newtonsoft.Json.FluentConfiguration.Settings;

public class SerializationSettings<T> : ISerializationSettings
{
    private List<Rule> rules { get; } = new List<Rule>();

    public IEnumerable<Rule> Rules { get; private set; }

    public SerializationSettings()
    {
        Rules = rules.AsEnumerable();
    }

    public PropertyRule<T, TProp> RuleFor<TProp>(Expression<Func<T, TProp>> prop)
    {
        var rule = new PropertyRule<T, TProp>(prop);
        rules.Add(rule);
        return rule;
    }
}
