public sealed record EitherSuccessResultModel<TSuccess>
{
    public required string Type { get; init; }
    public required string Detail { get; init; }

    public required TSuccess Success { get; init; } = default(TSuccess);
}
