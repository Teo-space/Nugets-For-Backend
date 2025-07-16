public readonly partial record struct Result
{
    public static implicit operator ErrorResult(Result result) => result.ToErrorResult();

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
