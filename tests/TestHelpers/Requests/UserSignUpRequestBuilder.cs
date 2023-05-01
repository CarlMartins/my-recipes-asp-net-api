using Bogus;
using RecipeBook.Comunication.Payloads;

namespace TestHelpers.Requests;

public static class UserSignUpRequestBuilder
{
    public static SignUpUserRequestDto Build(int passwordSize = 10)
    {
        return new Faker<SignUpUserRequestDto>()
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Password, f => f.Internet.Password(passwordSize))
            .RuleFor(c => c.Contact, f => f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(1, 9)}"));
    }
}