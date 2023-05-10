using Moq;
using RecipeBook.Domain.Repositories.User;

namespace TestHelpers.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    private readonly Mock<IUserWriteOnlyRepository> _repository;
    
    private UserWriteOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IUserWriteOnlyRepository>();
    }
    
    public static UserWriteOnlyRepositoryBuilder Instance()
    {
        return new UserWriteOnlyRepositoryBuilder();
    }
    
    public IUserWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}