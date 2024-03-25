using System.Security.Claims;

namespace Api.Auth.Jwt.UseJwtTokens.UserAccessor;

internal class TestUserAccessor : IUser
{
    public ClaimsPrincipal User
    {
        get
        {
            return new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new Claim []
                {
                    new Claim("UserName", "TestUser"),
                    new Claim("Email", "test.user@gmail.com"),
                    new Claim("UserId", 100500.ToString()),
                })
            });
        }
    }

}
