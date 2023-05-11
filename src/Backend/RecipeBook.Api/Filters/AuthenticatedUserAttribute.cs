using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using RecipeBook.Application.Services.Token;
using RecipeBook.Comunication.DTOs.Errors;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Exceptions;

namespace RecipeBook.Api.Filters;

public class AuthenticatedUserAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly ITokenController _tokenController;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public AuthenticatedUserAttribute(ITokenController tokenController, IUserReadOnlyRepository userReadOnlyRepository)
    {
        _tokenController = tokenController;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = RequestToken(context);
            var email = _tokenController.GetEmailFromToken(token);

            var user = await _userReadOnlyRepository.GetByEmail(email);
        
            if (user is null)
                throw new Exception();
        }
        catch (SecurityTokenExpiredException)
        {
            ExpiredToken(context);
        }
        catch (Exception)
        {
            UnauthorizedUser(context);
        }
    }

    private string RequestToken(AuthorizationFilterContext context)
    {
        var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();
        
        if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
            throw new Exception();
        
        return authorization["Bearer ".Length..];
    }
    
    private void ExpiredToken(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ErrorResponseDto(ResourceErrorMessages.EXPIRED_TOKEN));
    }
    
    private void UnauthorizedUser(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ErrorResponseDto(ResourceErrorMessages.UNAUTHORIZED_USER));
    }
}