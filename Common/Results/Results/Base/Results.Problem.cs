public static partial class Results
{
    public static Result<T> Problem<T>(T Value, string Type, string Detail)
        => Results.Create<T>(Value, false, Type, Detail);

    public static Result<T> Problem<T>(string Type, string Detail)
        => Results.Create<T>(default, false, Type, Detail);

    public static Result<T> Problem<T>(T Value, string Type, string Detail, 
        IReadOnlyCollection<FieldError> fieldErrors)
        => Results.Create<T>(Value, false, Type, Detail, fieldErrors);

    public static Result<T> Problem<T>(string Type, string Detail, IReadOnlyCollection<FieldError> fieldErrors)
        => Results.Create<T>(default, false, Type, Detail, fieldErrors);

}
