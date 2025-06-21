public static partial class Results
{
    public static Result<TMapped> MapTo<TSource, TMapped>(this Result<TSource> sourceResult, TMapped Value)
        => Results.Create<TMapped>(Value, sourceResult.Success, sourceResult.Type, sourceResult.Detail);
    public static Result<TMapped> MapTo<TSource, TMapped>(this Result<TSource> sourceResult, string detail, TMapped Value)
        => Results.Create<TMapped>(Value, sourceResult.Success, sourceResult.Type, detail);

    //--------------------------------------------------------------------------------------------------------

    public static Result<TMapped> MapTo<TSource, TMapped>(this Result<TSource> sourceResult)
        => Results.Create<TMapped>(default, sourceResult.Success, sourceResult.Type, sourceResult.Detail);
    public static Result<TMapped> MapTo<TSource, TMapped>(this Result<TSource> sourceResult, string detail)
        => Results.Create<TMapped>(default, sourceResult.Success, sourceResult.Type, detail);

}
