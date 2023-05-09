using FluentValidation;
using RecipeBook.Comunication.DTOs.Login;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UseCases.Login.LogIn;

public class LoginValidator : AbstractValidator<RequestLoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_USER_PASSWORD);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_USER_PASSWORD);

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(ResourceErrorMessages.INVALID_USER_EMAIL);
        });
        
        When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
        {
            RuleFor(x => x.Password.Length)
                .GreaterThanOrEqualTo(6)
                .WithMessage(ResourceErrorMessages.MINIMUN_SIX_CHARACTERS_PASSWORD);
        });
    }
}