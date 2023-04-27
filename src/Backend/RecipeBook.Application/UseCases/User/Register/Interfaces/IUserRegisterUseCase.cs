using RecipeBook.Comunication.Payloads;

namespace RecipeBook.Application.UseCases.User.Register.Interfaces;

public interface IUserRegisterUseCase
{
    Task Execute(SignUpUserRequestDto request);
}