using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UseCases.User.Register.Interfaces;
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
}