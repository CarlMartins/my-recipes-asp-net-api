using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.UserRepositories;

namespace RecipeBook.Infrastructure.RepositoryAccess.Repositories;

public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly MyRecipesContext _context;

    public UserRepository(MyRecipesContext context)
    {
        _context = context;
    }
    
    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<bool> AlreadyExistsUserWithEmail(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<User?> RecoverWithEmailAndPassword(string email, string password)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
    }
}