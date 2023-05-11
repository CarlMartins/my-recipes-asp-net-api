using Microsoft.AspNetCore.Mvc;
using RecipeBook.Api.Filters;
using RecipeBook.Application.UseCases.PasswordReset;
using RecipeBook.Application.UseCases.User.Register.Interfaces;
using RecipeBook.Comunication.DTOs.PasswordReset;
using RecipeBook.Comunication.DTOs.SignUp;

namespace RecipeBook.Api.Controllers;

public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseSignedUpUserDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> UserRegister(
        [FromServices] IUserRegisterUseCase useCase,
        [FromBody] SignUpUserRequestDto payload)
    {
        var result = await useCase.Execute(payload);

        return Created(string.Empty, result);
    }
    
    [HttpPut("password-reset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ServiceFilter(typeof(AuthenticatedUserAttribute))]
    public async Task<IActionResult> PasswordReset(
        [FromServices] IPasswordResetUseCase useCase,
        [FromBody] RequestPasswordResetDto request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
    
}