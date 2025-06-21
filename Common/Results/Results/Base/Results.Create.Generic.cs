public static partial class Results
{
    internal static Result<T> Create<T>(T Value, bool Success, string Type, string Detail) => new Result<T>()
    {
        Value = Value,
        Success = Success,
        Type = Type,
        Detail = Detail,
        Errors = Array.Empty<string>(),
    };

    internal static Result<T> Create<T>(T Value, bool Success, string Type, string Detail,
        IReadOnlyCollection<string> errors) => new Result<T>()
        {
            Value = Value,
            Success = Success,
            Type = Type,
            Detail = Detail,
            Errors = errors,
        };

}
