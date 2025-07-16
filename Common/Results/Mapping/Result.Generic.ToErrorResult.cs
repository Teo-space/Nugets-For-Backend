public readonly partial record struct Result<T>
{
    public static implicit operator ErrorResult(Result<T> result) => result.ToErrorResult();

    public ErrorResult ToErrorResult()
    {
        return new ErrorResult
        {
            Type = this.Type,
            Detail = this.Detail,
            Errors = this.Errors
        };
    }
}
