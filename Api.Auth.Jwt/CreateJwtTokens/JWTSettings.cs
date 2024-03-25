using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Auth.Jwt.CreateJwtTokens;

public class JWTSettings
{
    public string Secret { get; set; }
    public int ExpirationInMinutes { get; set; }


    public JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));

        var token = new JwtSecurityToken(
            //issuer: "Test",
            claims: authClaims,
            expires: DateTime.Now.AddMinutes(ExpirationInMinutes),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    public string WriteToken(List<Claim> authClaims)
    {
        var JwtSecurityToken = CreateToken(authClaims);

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(JwtSecurityToken);

        return token;
    }
}
