public sealed record ErrorResult
{
    public required string Type { get; init; }
    public required string Detail { get; init; }

    public required IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();
}
