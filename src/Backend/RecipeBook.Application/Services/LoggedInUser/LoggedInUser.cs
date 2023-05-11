using Microsoft.AspNetCore.Http;
using RecipeBook.Application.Services.Token;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.User;

namespace RecipeBook.Application.Services.LoggedInUser;

public class LoggedInUser : ILoggedInUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenController _tokenController;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    
    public LoggedInUser(
        IHttpContextAccessor httpContextAccessor, 
        ITokenController tokenController,
        IUserReadOnlyRepository readOnlyRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _tokenController = tokenController;
        _readOnlyRepository = readOnlyRepository;
    }

    public async Task<User?> GetUser()
    {
        var authorization = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

        var token = authorization!["Bearer".Length..].Trim();

        var email = _tokenController.GetEmailFromToken(token);

        var user = await _readOnlyRepository.GetByEmail(email);

        return user;
    }
}