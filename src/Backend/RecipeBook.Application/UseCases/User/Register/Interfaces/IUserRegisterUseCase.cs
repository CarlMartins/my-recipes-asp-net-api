using RecipeBook.Comunication.DTOs.SignUp;

namespace RecipeBook.Application.UseCases.User.Register.Interfaces;

public interface IUserRegisterUseCase
{
    Task<ResponseSignedUpUserDto> Execute(SignUpUserRequestDto request);
}