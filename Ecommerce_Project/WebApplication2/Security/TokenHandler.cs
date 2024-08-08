using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Security;

public static class TokenHandler
{
    public static Token CreateToken(IConfiguration configuration)
    {
        Token token = new();

        SymmetricSecurityKey securityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes((configuration["Token:SecurityKey"])));
            
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        JwtSecurityToken jwtSecurityToken = new(
            issuer: configuration["Token:Issuer"],
            audience: configuration["Token:Audience"],
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        JwtSecurityTokenHandler tokenHandler = new();
        token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);

        byte[] numbers = new byte[32];
        using RandomNumberGenerator random= RandomNumberGenerator.Create();
        random.GetBytes(numbers);
        token.RefreshToken = Convert.ToBase64String(numbers);
          
        {
            
        }
        
        return token; 

    }
}