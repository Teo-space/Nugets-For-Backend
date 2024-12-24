using System.Linq.Expressions;

namespace Newtonsoft.Json.FluentConfiguration.Models;

public class PropertyRule<TClass, TProp> : PropertyRule
{
    public const string CONVERTER_KEY = "Converter";
    public const string PROPERTY_NAME_KEY = "PropertyName";
    public const string IGNORED_KEY = "Ignored";

    public PropertyRule(Expression<Func<TClass, TProp>> prop)
    {
        PropertyInfo = (prop.Body as MemberExpression).Member;
    }

    public PropertyRule<TClass, TProp> Converter(JsonConverter converter)
    {
        AddRule(CONVERTER_KEY, converter);
        return this;
    }

    public PropertyRule<TClass, TProp> Name(string propertyName)
    {
        AddRule(PROPERTY_NAME_KEY, propertyName);
        return this;
    }

    public PropertyRule<TClass, TProp> Ignore()
    {
        AddRule(IGNORED_KEY, true);
        return this;
    }
}
