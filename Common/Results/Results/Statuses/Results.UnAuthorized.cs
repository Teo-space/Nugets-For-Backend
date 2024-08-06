public static partial class Results
{
	public static Result<T> UnAuthorizedResult<T>(string Detail)
		=> Problem<T>(Statuses.Unauthorized, Detail);

	public static Result<T> NotEnoughPermissions<T>(string Detail)
		=> Problem<T>(Statuses.NotEnoughPermissions, Detail);

	public static Result<T> NotEnoughPermissions<T>()
		=> Problem<T>(Statuses.NotEnoughPermissions, "Not Enought Permissions for action");




}