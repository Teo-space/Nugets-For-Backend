public static partial class Results
{
    public static Result Bad(string Detail) 
        => Problem(Statuses.BadRequest, Detail);

    public static Result Bad(string Detail, IReadOnlyCollection<string> errors) 
        => Problem(Statuses.BadRequest, Detail, errors);

    //--------------------------------------------------------------------------------------------------------

    public static Result<T> Bad<T>(T Value, string Detail) 
        => Problem<T>(Value, Statuses.BadRequest, Detail);

    public static Result<T> Bad<T>(T Value, string Detail, IReadOnlyCollection<string> errors)
        => Problem<T>(Value, Statuses.Conflict, Detail, errors);

    public static Result<T> Bad<T>(string Detail) 
        => Problem<T>(Statuses.BadRequest, Detail);

    public static Result<T> Bad<T>(string Detail, IReadOnlyCollection<string> errors) 
        => Problem<T>(Statuses.BadRequest, Detail, errors);

    //--------------------------------------------------------------------------------------------------------

    public static Result<TSuccess, TError> Bad<TSuccess, TError>(string Detail)
        => Problem<TSuccess, TError>(Statuses.BadRequest, Detail);

    public static Result<TSuccess, TError> Bad<TSuccess, TError>(string Detail, TError OnError)
        => Problem<TSuccess, TError>(Statuses.BadRequest, Detail, OnError);

}
