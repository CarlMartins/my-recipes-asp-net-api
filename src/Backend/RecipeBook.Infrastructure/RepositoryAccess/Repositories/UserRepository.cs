using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.User;

namespace RecipeBook.Infrastructure.RepositoryAccess.Repositories;

public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
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

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public async Task<User?> GetById(long id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}