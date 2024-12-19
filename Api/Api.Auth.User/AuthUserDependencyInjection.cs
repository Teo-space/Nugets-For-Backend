using Api.Auth.User.UserAccessor;
using Microsoft.Extensions.DependencyInjection;

public static class AuthUserDependencyInjection
{
    public static void AddUserAccessor(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUser, UserAccessor>();
    }

    public static void AddTestUserAccessor(this IServiceCollection services)
    {
        services.AddScoped<IUser, TestUserAccessor>();
    }

}