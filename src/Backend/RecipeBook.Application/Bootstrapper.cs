using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Services.Cryptography;
using RecipeBook.Application.Services.Token;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Application.UseCases.User.Register.Interfaces;

namespace RecipeBook.Application;

public static class Bootstrapper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddPasswordSalt(services, configuration);
        AddTokenController(services, configuration);
        services.AddScoped<IUserRegisterUseCase, UserRegisterUseCase>();
    }

    private static void AddPasswordSalt(this IServiceCollection services, IConfiguration configuration)
    {
        var passwordSaltSection = configuration.GetRequiredSection("Settings:PasswordSalt");

        services.AddSingleton<IPasswordEncryptor>(_ =>
            new PasswordEncryptor(passwordSaltSection.Value!));
    }

    private static void AddTokenController(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenExpirationMinutes = Convert.ToDouble(
            configuration.GetRequiredSection("Settings:TokenExpirationMinutes").Value);

        var tokenKey = configuration.GetRequiredSection("Settings:TokenKey").Value!;

        services.AddSingleton<ITokenController>(_ => new TokenController(tokenExpirationMinutes, tokenKey!));
    }
}