public partial record Result<T>
{
    public static implicit operator T(Result<T> Result) => Result.Value;
    public static implicit operator Result<T>(T o) => Results.Ok<T>(o);

    public static implicit operator Result<T>(Result result) => Results.Create<T>(default, result.Success, result.Type, result.Detail, result.Errors);

    public static implicit operator Result(Result<T> result) => Results.Create(result.Success, result.Type, result.Detail, result.Errors);
}