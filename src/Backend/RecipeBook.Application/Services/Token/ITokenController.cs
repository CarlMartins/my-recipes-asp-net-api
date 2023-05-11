using System.Security.Claims;

namespace RecipeBook.Application.Services.Token;

public interface ITokenController
{
    string GenerateToken(string email);
    ClaimsPrincipal TokenValidation(string token);
    string GetEmailFromToken(string token);
}