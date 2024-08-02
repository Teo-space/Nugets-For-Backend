public partial class Results
{
	public static Result<T> NotFound<T>(string Type, string Detail)
		=> Problem<T>(Type, Detail);
	public static Result<T> NotFound<T>(string Detail)
		=> Problem<T>(Statuses.NotFound, Detail);


	public static Result<T> NotFoundById<T>(object Id)
		=> NotFound<T>($"Not found {typeof(T).Name} By Id: {Id}");

	public static Result<T> ParentNotFound<T>(string Detail)
		=> NotFound<T>("ParentNotFound", Detail);
	public static Result<T> ParentNotFoundById<T>(object Id)
		=> ParentNotFound<T>($"Parent Not found {typeof(T).Name} By Id: {Id}");

}