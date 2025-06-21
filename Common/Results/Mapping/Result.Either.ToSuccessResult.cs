public partial record Result<TSuccess, TError>
{
    public static implicit operator EitherSuccessResultModel<TSuccess>(Result<TSuccess, TError> result) => result.ToSuccessResult();

    public EitherSuccessResultModel<TSuccess> ToSuccessResult()
    {
        return new EitherSuccessResultModel<TSuccess>
        {
            Type = this.Type,
            Detail = this.Detail,
            Success = this.OnSuccess,
        };
    }
}
