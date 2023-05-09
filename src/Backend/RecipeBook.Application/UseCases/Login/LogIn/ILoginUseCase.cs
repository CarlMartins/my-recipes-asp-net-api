using RecipeBook.Comunication.DTOs.Login;

namespace RecipeBook.Application.UseCases.Login.LogIn;

public interface ILoginUseCase
{
    Task<ResponseLoginDto> Execute(RequestLoginDto request);
}