public static partial class Results
{
    public static Result Conflict(string Detail) 
        => Problem(Statuses.Conflict, Detail);

    public static Result Conflict(string Detail, IReadOnlyCollection<string> errors) 
        => Problem(Statuses.Conflict, Detail, errors);

    //--------------------------------------------------------------------------------------------------------

    public static Result<T> Conflict<T>(T Value, string Detail) 
        => Problem<T>(Value, Statuses.Conflict, Detail);

    public static Result<T> Conflict<T>(T Value, string Detail, IReadOnlyCollection<string> errors)
        => Problem<T>(Value, Statuses.Conflict, Detail, errors);

    public static Result<T> Conflict<T>(string Detail) 
        => Problem<T>(Statuses.Conflict, Detail);

    public static Result<T> Conflict<T>(string Detail, IReadOnlyCollection<string> errors)
        => Problem<T>(Statuses.Conflict, Detail, errors);

    //--------------------------------------------------------------------------------------------------------

    public static Result<TSuccess, TError> Conflict<TSuccess, TError>(string Detail)
    => Problem<TSuccess, TError>(Statuses.Conflict, Detail);

    public static Result<TSuccess, TError> Conflict<TSuccess, TError>(string Detail, TError OnError)
        => Problem<TSuccess, TError>(Statuses.Conflict, Detail, OnError);
}
