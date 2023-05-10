using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.UserRepositories;

namespace TestHelpers.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IUserReadOnlyRepository>();
    }
    
    public static UserReadOnlyRepositoryBuilder Instance()
    {
        return new UserReadOnlyRepositoryBuilder();
    }

    public UserReadOnlyRepositoryBuilder AlreadyExistsUserWithEmail(string email)
    {
        if (!string.IsNullOrEmpty(email))
            _repository.Setup(i => i.AlreadyExistsUserWithEmail(email)).ReturnsAsync(true);
        
        return this;
    }
    
    public UserReadOnlyRepositoryBuilder RecoverWithEmailAndPassword(User user)
    {
        _repository.Setup(i => i.RecoverWithEmailAndPassword(user.Email, user.Password))
            .ReturnsAsync(user);
        
        return this;
    }
    
    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}