namespace Api.Correlation;

internal sealed class CorrelationService : ICorrelationService
{
    public Ulid AsUlid { get; init; } = Ulid.NewUlid();
    public Guid AsGuid => AsUlid.ToGuid();
    public string AsString => AsUlid.ToString();

    public byte[] AsBytes => AsUlid.ToByteArray();

    /*
    public CorrelationService()
    {
        AsUlid = Ulid.NewUlid();
    }
    */
}
