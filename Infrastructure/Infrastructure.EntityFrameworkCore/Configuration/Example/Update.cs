namespace Infrastructure.EntityFrameworkCore.Configuration.Example;

/// <summary>
/// Value Object
/// </summary>
public sealed record Update(int userId)
{
    public DateTime Date { get; private set; } = DateTime.Now;
    public int UserId { get; private set; } = userId;

    public void Updated(int userId)
    {
        Date = DateTime.Now;
        UserId = userId;
    }
}
