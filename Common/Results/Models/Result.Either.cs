public readonly partial record struct Result<TSuccess, TError>()
{
    public required bool Success { get; init; }
    public required string Type { get; init; }
    public required string Detail { get; init; }

    public required TSuccess OnSuccess { get; init; }
    public required TError OnError { get; init; }
}
