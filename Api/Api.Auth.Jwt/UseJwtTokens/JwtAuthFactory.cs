using Api.Auth.Jwt.UseJwtTokens.UserAccessor;

namespace Api.Auth.Jwt.UseJwtTokens;

public static class JwtAuthFactory
{
    public static IUser GetTestUser() => new TestUserAccessor();
}
