namespace RecipeBook.Domain.Repositories.UserRepositories;

public interface IUserReadOnlyRepository
{
    Task<bool> AlreadyExistsUserWithEmail(string email);
}