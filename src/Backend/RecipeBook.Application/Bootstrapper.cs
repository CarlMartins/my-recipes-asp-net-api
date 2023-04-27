using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Application.UseCases.User.Register.Interfaces;

namespace RecipeBook.Application;

public static class Bootstrapper
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserRegisterUseCase, UserRegisterUseCase>();
    }
}