using System.Text.RegularExpressions;
using FluentValidation;
using RecipeBook.Comunication.Payloads;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UseCases.User;

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
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_USER_PASSWORD);

        When(x => !string.IsNullOrEmpty(x.Password.Trim()), () =>
        {
            RuleFor(x => x.Password.Length).GreaterThanOrEqualTo(6)
                .WithMessage(ResourceErrorMessages.MINIMUN_SIX_CHARACTERS_PASSWORD);
        });

        When(x => !string.IsNullOrEmpty(x.Contact.Trim()), () =>
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