namespace RecipeBook.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> AlreadyExistsUserWithEmail(string email);
    Task<Entities.User?> RecoverWithEmailAndPassword(string email, string password);
    Task<Entities.User?> GetByEmail(string email);
}