using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UseCases.User;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Application.UseCases.User.Register.Interfaces;
using RecipeBook.Comunication.Payloads;

namespace RecipeBook.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get([FromServices] IUserRegisterUseCase registerUseCase)
    {
        var response = await registerUseCase.Execute(new SignUpUserRequestDto
        {
            Name = "Carlos",
            Email = "carlos@email.com",
            Password = "123456",
            Contact = "99 9 9999-9999"
        });

        return Ok(response);
    }
}