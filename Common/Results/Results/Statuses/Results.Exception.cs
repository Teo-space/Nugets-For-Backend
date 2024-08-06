public static partial class Results
{
	public static Result<T> Exception<T>(string Detail) 
		=> Problem<T>(Statuses.InternalServerError, Detail);


}