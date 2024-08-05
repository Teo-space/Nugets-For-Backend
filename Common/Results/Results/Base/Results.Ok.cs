public partial class Results
{
	public static Result<T> Ok<T>(T Value) 
		=> Results.Create<T>(Value, true, Statuses.Ok, default);

	public static Result<T> Ok<T>(T Value, string detail) 
		=> Results.Create<T>(Value, true, Statuses.Ok, detail);

}
