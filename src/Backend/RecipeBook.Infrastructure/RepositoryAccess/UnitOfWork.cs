using RecipeBook.Domain.Repositories;

namespace RecipeBook.Infrastructure.RepositoryAccess;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly MyRecipesContext _context;
    private bool _disposed = false;

    public UnitOfWork(MyRecipesContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        Dispose(true);
    }
    
    public void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }
}