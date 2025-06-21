public static partial class Results
{
    public static Result NoContent() 
        => Results.Create(true, Statuses.NoContent, default);

    //--------------------------------------------------------------------------------------------------------

    public static Result<T> NoContent<T>()
        => Results.Create<T>(default, true, Statuses.NoContent, default);

    public static Result<T> NoContent<T>(string detail)
        => Results.Create<T>(default, true, Statuses.NoContent, detail);

}
