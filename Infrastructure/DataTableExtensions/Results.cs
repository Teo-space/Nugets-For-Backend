namespace DataTableExtensions;

public static class Results
{
    public static Result<TResult> Ok<TResult>(TResult Value)
    {
        return new Result<TResult>
        {
            Value = Value,
            Success = true,
            Message = string.Empty,
            Errors = Array.Empty<string>()
        };
    }

    public static Result<TResult> Error<TResult>(string message) => Error<TResult>(message, Array.Empty<string>());
    public static Result<TResult> Error<TResult>(string message, IReadOnlyCollection<string> errors)
    {
        return new Result<TResult>
        {
            Value = default,
            Success = false,
            Message = message,
            Errors = errors
        };
    }



}
