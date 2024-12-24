using Newtonsoft.Json.FluentConfiguration.Settings;

namespace Newtonsoft.Json.FluentConfiguration.Example;

public class OrderFJConfig : SerializationSettings<Order>
{
    public OrderFJConfig()
    {
        RuleFor(x => x.Positions).Ignore();
    }
}
