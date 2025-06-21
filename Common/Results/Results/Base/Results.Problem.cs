public static partial class Results
{
    public static Result Problem(string Type, string Detail) => Results.Create(false, Type, Detail);

    public static Result Problem(string Type, string Detail, IReadOnlyCollection<string> errors)
        => Results.Create(false, Type, Detail, errors);

}
