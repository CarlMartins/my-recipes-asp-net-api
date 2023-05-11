using System.Text.RegularExpressions;
using FluentValidation;
using RecipeBook.Comunication.DTOs.SignUp;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UseCases.User.Register;

public class UserSignUpValidator : AbstractValidator<SignUpUserRequestDto>
{
    public UserSignUpValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_USER_NAME);
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_USER_EMAIL);
        
        RuleFor(x => x.Contact)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_USER_CONTACT);

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(ResourceErrorMessages.INVALID_USER_EMAIL);
        });

        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());
      
        When(x => !string.IsNullOrWhiteSpace(x.Contact), () =>
        {
            RuleFor(x => x.Contact).Custom((contact, context) =>
            {
                string telephonePattern = "[0-9]{2} [1-9]{1} [0-9]{4}-[0-9]{4}";
                var isMatch = Regex.IsMatch(contact, telephonePattern);

                if (!isMatch)
                {
                    context.AddFailure(ResourceErrorMessages.INVALID_USER_CONTACT);
                }
            });
        });
    }
}