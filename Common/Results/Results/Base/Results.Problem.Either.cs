public static partial class Results
{
    public static Result<TSuccess, TError> Problem<TSuccess, TError>(string Type, string Detail)
        => Results.Create<TSuccess, TError>(false, Type, Detail, default(TError));

    public static Result<TSuccess, TError> Problem<TSuccess, TError>(string Type, string Detail, TError OnError)
        => Results.Create<TSuccess, TError>(false, Type, Detail, OnError);

}
