public static partial class Results
{
    internal static Result Create(bool Success, string Type, string Detail) => new Result
    {
        Success = Success,
        Type = Type,
        Detail = Detail,
        Errors = Array.Empty<string>()
    };

    internal static Result Create(bool Success, string Type, string Detail, IReadOnlyCollection<string> errors) => new Result
    {
        Success = Success,
        Type = Type,
        Detail = Detail,
        Errors = errors
    };

}
