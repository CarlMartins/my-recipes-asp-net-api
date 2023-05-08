using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Repositories.UserRepositories;

public interface IUserReadOnlyRepository
{
    Task<bool> AlreadyExistsUserWithEmail(string email);
    Task<User?> RecoverWithEmailAndPassword(string email, string password);
}