using Api.Auth.Jwt.UseJwtTokens.UserAccessor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class JwtAuthDependencyInjection
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

    public static void AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
    {
        var JWTSettings = configuration.GetSection("JWTSettings");
        var jwtSecret = JWTSettings.GetValue<string>("Secret")
            ?? throw new Exception("jwtSecret not found ");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateAudience = false,
                 ValidateIssuer = false,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
             };
         });
    }

}