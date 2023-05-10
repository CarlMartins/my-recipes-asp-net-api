using Bogus;
using RecipeBook.Domain.Entities;
using TestHelpers.Encryptor;

namespace TestHelpers.Entities;

public class UserBuilder
{
    public static (User user, string password) Build()
    {
        var password = string.Empty;
        
        var createdUser = new Faker<User>()
            .RuleFor(c => c.Id, _ => 1)
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Password, f =>
            {
                password = f.Internet.Password();
                
                return PasswordEncryptorBuilder
                    .Instance()
                    .Encrypt(password);
            })
            .RuleFor(c => c.Contact, f => f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(1, 9)}"));
        
        return (createdUser, password);
    }
}