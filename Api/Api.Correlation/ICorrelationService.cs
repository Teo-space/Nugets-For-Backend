namespace Api.Correlation;

public interface ICorrelationService
{
    public Ulid AsUlid { get; }
    public Guid AsGuid { get; }
    public string AsString { get; }

    public byte[] AsBytes { get; }
}
