public static class RateLimiterPolicies
{
    public const string Fixed = nameof(Fixed);
    public const string Sliding = nameof(Sliding);
    public const string Token = nameof(Token);
    public const string Concurrency = nameof(Concurrency);

    public const string FixedByIp = nameof(FixedByIp);
    public const string FixedByForwardedIp = nameof(FixedByForwardedIp);
    public const string FixedByByIdentity = nameof(FixedByByIdentity);
    public const string FixedByByIpAndIdentity = nameof(FixedByByIpAndIdentity);





}
