public partial record Result<TSuccess, TError>
{
    public static implicit operator TSuccess(Result<TSuccess, TError> Result) => Result.OnSuccess;

    public static implicit operator Result<TSuccess, TError>(TSuccess o) => Results.Ok<TSuccess, TError>(o);
}