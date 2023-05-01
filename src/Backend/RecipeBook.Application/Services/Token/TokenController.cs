using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RecipeBook.Application.Services.Token;

public class TokenController : ITokenController
{
    private const string EmailAlias = "eml";
    private readonly double _tokenExpirationMinutes;
    private readonly string _tokenKey;

    public TokenController(double tokenExpirationMinutes, string tokenKey)
    {
        _tokenExpirationMinutes = tokenExpirationMinutes;
        _tokenKey = tokenKey;
    }
    
    public string GenerateToken(string email)
    {
        var claims = new List<Claim>
        {
            new(EmailAlias, email)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = SymmetricKey();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public void TokenValidation(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var validationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            IssuerSigningKey = SymmetricKey(),
            ClockSkew = new TimeSpan(0),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        
        tokenHandler.ValidateToken(token, validationParameters, out _);
    }
    
    private SymmetricSecurityKey SymmetricKey()
    {
        var symmetricKey = Convert.FromBase64String(_tokenKey);
        return new SymmetricSecurityKey(symmetricKey);
    }
}