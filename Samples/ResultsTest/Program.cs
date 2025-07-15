// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");


var result = Results.Ok(new Order(1));




public record Order(long OrderId);



internal sealed record OrderService
{
    public Result<Order> Create()
    {
        return Results.Conflict("Error");
    }
}