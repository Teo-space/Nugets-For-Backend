public static partial class Results
{
    public static Result Exception(string Detail) 
        => Problem(Statuses.InternalServerError, Detail);

    //--------------------------------------------------------------------------------------------------------

    public static Result<T> Exception<T>(T Value, string Detail) 
        => Problem<T>(Value, Statuses.InternalServerError, Detail);

    public static Result<T> Exception<T>(string Detail) 
        => Problem<T>(Statuses.InternalServerError, Detail);

    //--------------------------------------------------------------------------------------------------------

    public static Result<TSuccess, TError> Exception<TSuccess, TError>(string Detail)
        => Problem<TSuccess, TError>(Statuses.InternalServerError, Detail);

    public static Result<TSuccess, TError> Exception<TSuccess, TError>(string Detail, TError OnError)
        => Problem<TSuccess, TError>(Statuses.InternalServerError, Detail, OnError);
}
