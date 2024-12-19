using Api.Auth.User.UserAccessor;

namespace Api.Auth.User;

public static class AuthUserFactory
{
    public static IUser GetTestUser() => new TestUserAccessor();
}
