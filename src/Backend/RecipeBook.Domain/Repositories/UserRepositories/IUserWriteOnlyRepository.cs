using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Repositories.UserRepositories;

public interface IUserWriteOnlyRepository
{
    Task Add(User user);
}