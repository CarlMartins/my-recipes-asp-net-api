using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UseCases.Login.LogIn;
using RecipeBook.Comunication.DTOs.Login;

namespace RecipeBook.Api.Controllers;

public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseLoginDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginUseCase useCase,
        [FromBody] RequestLoginDto request)
    {
        var response = await useCase.Execute(request);
        
        return Ok(response);
    }
}