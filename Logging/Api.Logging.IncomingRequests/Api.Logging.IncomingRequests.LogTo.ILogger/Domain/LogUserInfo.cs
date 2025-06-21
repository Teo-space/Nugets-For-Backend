namespace Api.Logging.IncomingRequests.LogTo.ILogger.Domain;

public sealed record LogUserInfo
{
    public string UserId { get; set; }
    public string IdentityName { get; set; }
}