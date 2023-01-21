using RecipeBook.Comunication.Payloads;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User;

public class UserRegisterUseCase
{
    public async Task Execute(SignUpUserRequestDto request)
    {
        Validate(request);
    }

    private void Validate(SignUpUserRequestDto request)
    {
        var validator = new UserSignUpValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ValidationErrorsException(errorMessages);
        }
    }
}