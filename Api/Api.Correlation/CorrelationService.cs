namespace Api.Correlation;

internal sealed class CorrelationService : ICorrelationService
{
    public Ulid Ulid { get; init; }
    public Guid Guid => Ulid.ToGuid();
    public string String => Ulid.ToString();

    public byte[] Bytes => Ulid.ToByteArray();


    public CorrelationService()
    {
        Ulid = new Ulid();
    }
}
