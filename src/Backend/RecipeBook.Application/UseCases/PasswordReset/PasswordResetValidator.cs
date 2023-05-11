using FluentValidation;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Comunication.DTOs.PasswordReset;

namespace RecipeBook.Application.UseCases.PasswordReset;

public class PasswordResetValidator : AbstractValidator<RequestPasswordResetDto>
{
    public PasswordResetValidator()
    {
        RuleFor(x => x.NewPassword)
            .SetValidator(new PasswordValidator());
    }
}