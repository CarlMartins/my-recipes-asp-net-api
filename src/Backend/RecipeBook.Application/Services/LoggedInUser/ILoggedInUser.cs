using RecipeBook.Domain.Entities;

namespace RecipeBook.Application.Services.LoggedInUser;

public interface ILoggedInUser
{
    Task<User?> GetUser();
}