using RecipeBook.Comunication.DTOs.PasswordReset;

namespace RecipeBook.Application.UseCases.PasswordReset;

public interface IPasswordResetUseCase
{
    Task Execute(RequestPasswordResetDto request);
}