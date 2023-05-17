using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.User;

namespace TestHelpers.Repositories;

public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _repository;

    private UserUpdateOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IUserUpdateOnlyRepository>();
    }

    public static UserUpdateOnlyRepositoryBuilder Instance()
    {
        return new UserUpdateOnlyRepositoryBuilder();
    }

    public UserUpdateOnlyRepositoryBuilder GetById(User user)
    {
        _repository.Setup(c => c.GetById(user.Id)).ReturnsAsync(user);
        return this;
    }

    public IUserUpdateOnlyRepository Build()
    {
        return _repository.Object;
    }
}