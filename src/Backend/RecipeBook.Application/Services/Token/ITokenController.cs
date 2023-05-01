namespace RecipeBook.Application.Services.Token;

public interface ITokenController
{
    string GenerateToken(string email);
    void TokenValidation(string token);
}