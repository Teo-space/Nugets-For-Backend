using System.Security.Claims;

namespace Api.Auth.Jwt.UseJwtTokens.UserAccessor;

/// <summary>
/// HttpContext ClaimsPrincipal User
/// </summary>
public interface IUser
{
    public abstract ClaimsPrincipal User { get; }

    public virtual string UserName => FindFirst("UserName")?.Value
        ?? throw new UnauthorizedAccessException("User has no claim 'UserName'");

    public virtual string Email => FindFirst("Email")?.Value
        ?? throw new UnauthorizedAccessException("User has no claim 'Email'");


    public virtual int UserId
    {
        get
        {
            var userId = FindFirst("UserId")?.Value
                ?? throw new UnauthorizedAccessException("User has no claim 'UserId'");

            return int.Parse(userId);
        }
    }



    public virtual IEnumerable<Claim> Claims => User.Claims;

    public virtual bool IsInRole(string role) => User.IsInRole(role);

    public virtual IEnumerable<Claim> FindAll(Predicate<Claim> match) => User.FindAll(match);
    public virtual IEnumerable<Claim> FindAll(string type) => User.FindAll(type);

    public virtual Claim FindFirst(Predicate<Claim> match) => User.FindFirst(match);
    public virtual Claim FindFirst(string type) => User.FindFirst(type);

    public virtual bool HasClaim(Predicate<Claim> match) => User.HasClaim(match);
    public virtual bool HasClaim(string type, string value) => User.HasClaim(type, value);
}