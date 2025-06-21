public partial record Result<T>()
{
    public required T Value { get; init; }

    public required bool Success { get; init; }
    public required string Type { get; init; }
    public required string Detail { get; init; }

    public required IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();

    public static implicit operator T(Result<T> Result) => Result.Value;
    public static implicit operator Result<T>(T o) => Results.Ok<T>(o);
}
