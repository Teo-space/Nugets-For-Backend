public sealed record EitherErrorResultModel<TError>
{
    public required string Type { get; init; }
    public required string Detail { get; init; }

    public required TError Error { get; init; } = default(TError);
}
