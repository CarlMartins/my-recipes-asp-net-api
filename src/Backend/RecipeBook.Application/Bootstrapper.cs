using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Services.Cryptography;
using RecipeBook.Application.Services.LoggedInUser;
using RecipeBook.Application.Services.Token;
using RecipeBook.Application.UseCases.Login.LogIn;
using RecipeBook.Application.UseCases.PasswordReset;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Application.UseCases.User.Register.Interfaces;

namespace RecipeBook.Application;

public static class Bootstrapper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddPasswordSalt(services, configuration);
        AddTokenController(services, configuration);
        AddUseCases(services);
        AddLoggedInUser(services);
    }

    private static void AddPasswordSalt(this IServiceCollection services, IConfiguration configuration)
    {
        var passwordSaltSection = configuration.GetRequiredSection("Settings:Password:PasswordSalt");
        
        services.AddSingleton<IPasswordEncryptor>(_ =>
            new PasswordEncryptor(passwordSaltSection.Value!));
    }

    private static void AddTokenController(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenExpirationMinutes = Convert.ToDouble(
            configuration.GetRequiredSection("Settings:Jwt:TokenExpirationMinutes").Value);

        var tokenKey = configuration.GetRequiredSection("Settings:Jwt:TokenKey").Value!;

        services.AddSingleton<ITokenController>(_ => new TokenController(tokenExpirationMinutes, tokenKey!));
    }
    
    private static void AddUseCases(this IServiceCollection services)
    {
        services
            .AddScoped<IUserRegisterUseCase, UserRegisterUseCase>()
            .AddScoped<ILoginUseCase, LoginUseCase>()
            .AddScoped<IPasswordResetUseCase, PasswordResetUseCase>();
    }
    
    private static void AddLoggedInUser(IServiceCollection services)
    {
        services.AddScoped<ILoggedInUser, LoggedInUser>();
    }
}