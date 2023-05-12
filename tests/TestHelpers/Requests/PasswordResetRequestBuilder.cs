using Bogus;
using RecipeBook.Comunication.DTOs.PasswordReset;

namespace TestHelpers.Requests;

public static class PasswordResetRequestBuilder
{
    public static RequestPasswordResetDto Build(int passwordLength = 10)
    {
        return new Faker<RequestPasswordResetDto>()
            .RuleFor(c => c.CurrentPassword, f => f.Internet.Password(passwordLength))
            .RuleFor(c => c.NewPassword, f => f.Internet.Password(passwordLength));
    }
}