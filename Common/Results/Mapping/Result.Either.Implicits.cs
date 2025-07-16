public readonly partial record struct Result<TSuccess, TError>
{
    public static implicit operator TSuccess(Result<TSuccess, TError> Result) => Result.OnSuccess;

    public static implicit operator Result<TSuccess, TError>(TSuccess o) => Results.Ok<TSuccess, TError>(o);

    public static implicit operator Result<TSuccess, TError>(Result<TSuccess> Result)
        => Results.Create<TSuccess, TError>(Result.Success, Result.Type, Result.Detail, Result.Value);

    public static implicit operator Result<TSuccess, TError>(Result<TError> Result)
        => Results.Create<TSuccess, TError>(Result.Success, Result.Type, Result.Detail, Result.Value);
}