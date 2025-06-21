public static class CheckEnvironment
{
    public static bool IsDevelopment =>
        Environment
        .GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        .IfIsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT"))
        .IfIsNullOrWhiteSpace("Production")
        .Equals("Development");

    public static string IfIsNullOrWhiteSpace(this string value, string otherValue)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return otherValue;
        }

        return value;
    }

    public static string IfIsNullOrEmpty(this string value, string otherValue)
    {
        if (string.IsNullOrEmpty(value))
        {
            return otherValue;
        }

        return value;
    }
}
