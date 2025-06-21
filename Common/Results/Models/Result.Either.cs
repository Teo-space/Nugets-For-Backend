public partial record Result<TSuccess, TError>
{
    public required bool Success { get; init; }
    public required string Type { get; init; }
    public required string Detail { get; init; }

    public required TSuccess OnSuccess { get; init; }
    public required TError OnError { get; init; }


    public static implicit operator TSuccess(Result<TSuccess, TError> Result) => Result.OnSuccess;

    public static implicit operator Result<TSuccess, TError>(TSuccess o) => Results.Ok<TSuccess, TError>(o);
}
