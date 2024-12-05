namespace Api.Date;

internal sealed record DateService : IDateService
{
    public DateTime Date { get; init; } 

    public DateService()
    {
        Date = DateTime.Now;
    }
}
