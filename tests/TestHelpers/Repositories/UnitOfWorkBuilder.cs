using Moq;
using RecipeBook.Domain.Repositories;

namespace TestHelpers.Repositories;

public class UnitOfWorkBuilder
{
    private readonly Mock<IUnitOfWork> _mock;
    
    private UnitOfWorkBuilder()
    {
        _mock ??= new Mock<IUnitOfWork>();
    }
    
    public static UnitOfWorkBuilder Instance()
    {
        return new UnitOfWorkBuilder();
    }
    
    public IUnitOfWork Build()
    {
        return _mock.Object;
    }
}