public readonly partial record struct Result<T>()
{
    public required T Value { get; init; }

    public required bool Success { get; init; }
    public required string Type { get; init; }
    public required string Detail { get; init; }

    public required IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();
}
