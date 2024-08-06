public partial class Results
{
    public static Result<TMapped> MapTo<TSource, TMapped>(Result<TSource> sourceResult)
        => Results.Create<TMapped>(default, sourceResult.Success, sourceResult.Type, sourceResult.Detail);
    public static Result<TMapped> MapTo<TSource, TMapped>(Result<TSource> sourceResult, string detail)
        => Results.Create<TMapped>(default, sourceResult.Success, sourceResult.Type, detail);

}