public readonly partial record struct Result<TSuccess, TError>
{
    public static implicit operator EitherErrorResultModel<TError>(Result<TSuccess, TError> result) => result.ToErrorResult();

    public EitherErrorResultModel<TError> ToErrorResult()
    {
        return new EitherErrorResultModel<TError>
        {
            Type = this.Type,
            Detail = this.Detail,
            Error = this.OnError,
        };
    }
}
