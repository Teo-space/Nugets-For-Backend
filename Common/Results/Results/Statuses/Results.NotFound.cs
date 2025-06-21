public static partial class Results
{
    public static Result NotFound(string Detail) 
        => Problem(Statuses.NotFound, Detail);

    //------------------------------------------------------------------------------------------

    public static Result<T> NotFound<T>(T Value, string Detail) 
        => Problem<T>(Value, Statuses.NotFound, Detail);

    public static Result<T> NotFound<T>(string Detail) 
        => Problem<T>(Statuses.NotFound, Detail);

    //--------------------------------------------------------------------------------------------------------

    public static Result<TSuccess, TError> NotFound<TSuccess, TError>(string Detail)
        => Problem<TSuccess, TError>(Statuses.NotFound, Detail);

    public static Result<TSuccess, TError> NotFound<TSuccess, TError>(string Detail, TError OnError)
        => Problem<TSuccess, TError>(Statuses.NotFound, Detail, OnError);
}
