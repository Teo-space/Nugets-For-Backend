namespace Api.Correlation;

public interface ICorrelationService
{
    public Ulid Ulid { get; }
    public Guid Guid { get; }
    public string String { get; }

    public byte[] Bytes { get; }
}
