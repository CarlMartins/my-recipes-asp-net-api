using Moq;
using RecipeBook.Application.Services.LoggedInUser;
using RecipeBook.Domain.Entities;

namespace TestHelpers.LoggedInUser;

public class LoggedInUserBuilder
{
    private readonly Mock<ILoggedInUser> _loggedInUser;

    public LoggedInUserBuilder()
    {
        _loggedInUser ??= new Mock<ILoggedInUser>();
    }
    
    public static LoggedInUserBuilder Instance()
    {
        return new LoggedInUserBuilder();
    }

    public LoggedInUserBuilder GetUser(User user)
    {
        _loggedInUser.Setup(c => c.GetUser()).ReturnsAsync(user);

        return this;
    }
    
    public ILoggedInUser Build()
    {
        return _loggedInUser.Object;
    }
}