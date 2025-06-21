public static partial class Results
{
    public static Result UnAuthorized(string Detail) 
        => Problem(Statuses.Unauthorized, Detail);

    public static Result NotEnoughPermissions(string Detail) 
        => Problem(Statuses.Unauthorized, Detail);

    //--------------------------------------------------------------------------------------------------------

    public static Result<T> UnAuthorized<T>(T Value, string Detail) 
        => Problem<T>(Statuses.Unauthorized, Detail);

    public static Result<T> UnAuthorized<T>(string Detail) 
        => Problem<T>(Statuses.Unauthorized, Detail);

    //--------------------------------------------------------------------------------------------------------

    public static Result<T> NotEnoughPermissions<T>(T Value, string Detail) 
        => Problem<T>(Statuses.NotEnoughPermissions, Detail);

    public static Result<T> NotEnoughPermissions<T>(string Detail) 
        => Problem<T>(Statuses.NotEnoughPermissions, Detail);

}
