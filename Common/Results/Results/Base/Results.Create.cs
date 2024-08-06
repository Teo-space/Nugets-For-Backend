public static partial class Results
{
    static readonly IReadOnlyCollection<FieldError> Empty = [];


    public static Result<T> Create<T>(T Value, 
        bool Success,
        string Type, string Detail, 
        IReadOnlyCollection<FieldError> fieldErrors) => new Result<T>()
    {
        Value = Value,
        Success = Success,
        Type = Type,
        Detail = Detail,
        Errors = fieldErrors
    };



    public static Result<T> Create<T>(T Value, bool Success, string Type, string Detail) => new Result<T>()
    {
        Value = Value,
        Success = Success,
        Type = Type,
        Detail = Detail,
        Errors = Empty
    };


}
