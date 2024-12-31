namespace DataTableExtensions;

public sealed record Result<T>
{
    public required bool Success { get; init; }

    public required T Value { get; init; }

    public required string Message { get; init; }
    public required IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();

}