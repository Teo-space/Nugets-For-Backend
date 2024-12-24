namespace Newtonsoft.Json.FluentConfiguration.Example;

public class Order
{
    public int OrderId { get; set; }

    public List<OrderPosition> Positions { get; set; } = new List<OrderPosition>();
}
