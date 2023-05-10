using RecipeBook.Domain.Entities;
using RecipeBook.Infrastructure.RepositoryAccess;
using TestHelpers.Entities;

namespace WebApi.Test;

public class ContextSeedInMemory
{
    public static (User user, string password) Seed(MyRecipesContext context)
    {
        var (user, password) = UserBuilder.Build();
        
        context.Users.Add(user);

        context.SaveChanges();

        return (user, password);
    }
}