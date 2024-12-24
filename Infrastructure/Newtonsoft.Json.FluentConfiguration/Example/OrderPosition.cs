namespace Newtonsoft.Json.FluentConfiguration.Example;

public class OrderPosition
{
    public int OrderPositionId { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }

}
