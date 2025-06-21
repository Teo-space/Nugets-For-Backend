public static partial class Results
{
    public static Result Deleted(string Detail) 
        => Problem(Statuses.Deleted, Detail);

    //--------------------------------------------------------------------------------------------------------

    public static Result<T> Deleted<T>(T Value, string Detail) 
        => Problem<T>(Value, Statuses.Deleted, Detail);

    public static Result<T> Deleted<T>(string Detail) 
        => Problem<T>(Statuses.Deleted, Detail);

    //--------------------------------------------------------------------------------------------------------

    public static Result<TSuccess, TError> Deleted<TSuccess, TError>(string Detail)
        => Problem<TSuccess, TError>(Statuses.Deleted, Detail);

    public static Result<TSuccess, TError> Deleted<TSuccess, TError>(string Detail, TError OnError)
        => Problem<TSuccess, TError>(Statuses.Deleted, Detail, OnError);
}
