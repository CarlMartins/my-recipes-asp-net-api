using FluentValidation;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UseCases.User.Register;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_USER_PASSWORD);

        When(x => !string.IsNullOrWhiteSpace(x), () =>
        {
            RuleFor(x => x.Length).GreaterThanOrEqualTo(6)
                .WithMessage(ResourceErrorMessages.MINIMUN_SIX_CHARACTERS_PASSWORD);
        });
    }
}