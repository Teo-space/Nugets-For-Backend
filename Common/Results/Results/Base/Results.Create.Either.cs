public static partial class Results
{
    internal static Result<TSuccess, TError> Create<TSuccess, TError>(bool Success, string Type, string Detail, TSuccess OnSuccess)
        => new Result<TSuccess, TError>()
        {
            Success = Success,
            Type = Type,
            Detail = Detail,
            OnSuccess = OnSuccess,
            OnError = default,
        };

    internal static Result<TSuccess, TError> Create<TSuccess, TError>(bool Success, string Type, string Detail, TError OnError)
        => new Result<TSuccess, TError>()
        {
            Success = Success,
            Type = Type,
            Detail = Detail,
            OnSuccess = default,
            OnError = OnError,
        };

}
