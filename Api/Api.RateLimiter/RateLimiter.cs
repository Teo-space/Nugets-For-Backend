using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;

/// <summary>
/// Варианты рейтлимитера
/// </summary>
/// Example:
[EnableRateLimiting(RateLimiterPolicies.Fixed)]
[DisableRateLimiting]
public static class RateLimiter
{
    /// <summary>
    /// app.UseRateLimiter();
    /// </summary>
    public static IServiceCollection AddRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddFixedWindowLimiter();
            options.AddSlidingWindowLimiter();
            options.AddTokenBucketLimiter();
            options.AddConcurrencyLimiter();
        });

        return services;
    }

    /// <summary>
    /// [EnableRateLimiting("fixed")]
    /// </summary>
    public static RateLimiterOptions AddFixedWindowLimiter(this RateLimiterOptions options)
    {
        options.AddFixedWindowLimiter(RateLimiterPolicies.Fixed, options =>
        {
            options.PermitLimit = 10;//Количество запросов в окне
            options.Window = TimeSpan.FromSeconds(10);//Размер окна
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = 5;
        });

        return options;
    }

    /// <summary>
    /// [EnableRateLimiting("sliding")]
    /// </summary>
    public static RateLimiterOptions AddSlidingWindowLimiter(this RateLimiterOptions options)
    {
        options.AddSlidingWindowLimiter(RateLimiterPolicies.Sliding, options =>
        {
            options.PermitLimit = 10;
            options.Window = TimeSpan.FromSeconds(10);
            options.SegmentsPerWindow = 2;
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = 5;
        });

        return options;
    }

    /// <summary>
    /// [EnableRateLimiting("token")]
    /// </summary>
    public static RateLimiterOptions AddTokenBucketLimiter(this RateLimiterOptions options)
    {
        options.AddTokenBucketLimiter(RateLimiterPolicies.Token, options =>
        {
            options.TokenLimit = 100;
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = 5;
            options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
            options.TokensPerPeriod = 20;
            options.AutoReplenishment = true;
        });

        return options;
    }

    /// <summary>
    /// [EnableRateLimiting("concurrency")]
    /// </summary>
    public static RateLimiterOptions AddConcurrencyLimiter(this RateLimiterOptions options)
    {
        options.AddConcurrencyLimiter(RateLimiterPolicies.Concurrency, options =>
        {
            options.PermitLimit = 10;
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = 5;
        });

        return options;
    }

    public static RateLimiterOptions AddFixedWindowLimiterByIp(this RateLimiterOptions options)
    {
        options.AddPolicy(RateLimiterPolicies.FixedByIp, httpContext =>
        {
            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                factory: x => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromMinutes(1)
                });
        });

        return options;
    }

    public static RateLimiterOptions AddFixedWindowLimiterByForwardedIp(this RateLimiterOptions options)
    {
        options.AddPolicy(RateLimiterPolicies.FixedByForwardedIp, httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                httpContext.Request.Headers["X-Forwarded-For"].ToString(),
                factory: x => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromMinutes(1)
                }));

        return options;
    }

    public static RateLimiterOptions AddFixedWindowLimiterByIdentity(this RateLimiterOptions options)
    {
        options.AddPolicy(RateLimiterPolicies.FixedByByIdentity, httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.User.Identity?.Name?.ToString(),
                factory: x => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromMinutes(1)
                }));

        return options;
    }

    public static RateLimiterOptions AddFixedWindowLimiterByIpAndIdentity(this RateLimiterOptions options)
    {
        options.AddPolicy(RateLimiterPolicies.FixedByByIpAndIdentity, httpContext =>
        {
            string Ip = httpContext.Request.Headers["X-Forwarded-For"].ToString() 
                ?? httpContext.Connection.RemoteIpAddress?.ToString()
                ?? string.Empty;
            string userName = httpContext.User.Identity?.Name?.ToString() ?? string.Empty;

            return
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: $"{Ip}.{userName}",
                factory: x => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromMinutes(1)
                });
        });

        return options;
    }

}
