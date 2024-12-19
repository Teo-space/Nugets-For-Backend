using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Api.Auth.User.UserAccessor;

internal class UserAccessor : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal User
        => _httpContextAccessor?.HttpContext?.User ?? throw new UnauthorizedAccessException("HttpContext.User is null");
}
