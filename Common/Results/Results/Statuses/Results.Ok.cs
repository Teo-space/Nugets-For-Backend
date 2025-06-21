public static partial class Results
{
    public static Result Ok(string detail) 
        => Results.Create(true, Statuses.Ok, detail);

    //------------------------------------------------------------------------------------------

    public static Result<T> Ok<T>(T Value) 
        => Results.Create<T>(Value, true, Statuses.Ok, default);

    public static Result<T> Ok<T>(T Value, string detail) 
        => Results.Create<T>(Value, true, Statuses.Ok, detail);

    //------------------------------------------------------------------------------------------

    public static Result<TSuccess, TError> Ok<TSuccess, TError>(TSuccess OnSuccess)
        => Create<TSuccess, TError>(true, Statuses.Ok, string.Empty, OnSuccess);

    public static Result<TSuccess, TError> Ok<TSuccess, TError>(string Detail, TSuccess OnSuccess)
        => Create<TSuccess, TError>(true, Statuses.Ok, Detail, OnSuccess);

}
