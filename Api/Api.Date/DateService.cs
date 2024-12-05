namespace Api.Date;

public sealed record DateService : IDateService
{
    public DateTime Date { get; init; } 

    public DateService()
    {
        Date = DateTime.Now;
    }
}
