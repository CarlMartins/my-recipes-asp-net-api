using RecipeBook.Comunication.Payloads;
using RecipeBook.Comunication.Responses;

namespace RecipeBook.Application.UseCases.User.Register.Interfaces;

public interface IUserRegisterUseCase
{
    Task<SignedUpUserDto> Execute(SignUpUserRequestDto request);
}