using FluentAssertions;
using RecipeBook.Application.UseCases.Login.LogIn;
using RecipeBook.Comunication.DTOs.Login;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using TestHelpers.Encryptor;
using TestHelpers.Entities;
using TestHelpers.Mapper;
using TestHelpers.Repositories;
using TestHelpers.Token;

namespace UseCases.Test.Login.LogIn;

public class LoginUseCaseTest
{
    [Fact]
    public async Task Login_Should_ReturnSuccess()
    {
        var (user, password) = UserBuilder.Build();

        var useCase = MakeUseCase(user);

        var request = new RequestLoginDto
        {
            Email = user.Email,
            Password = password
        };
        
        var response = await useCase.Execute(request);

        response.Should().NotBeNull();
        response.Name.Should().Be(user.Name);
        response.Token.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Login_Should_ReturnInvalidLoginException_WhenInvalidPassword()
    {
        var (user, _) = UserBuilder.Build();

        var useCase = MakeUseCase(user);

        var request = new RequestLoginDto
        {
            Email = user.Email,
            Password = "invalidPassword"
        };
        
        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<InvalidLoginException>()
            .Where(exception => exception.Message.Equals(ResourceErrorMessages.INVALID_LOGIN));
    }
    
    [Fact]
    public async Task Login_Should_ReturnInvalidLoginException_WhenInvalidEmail()
    {
        var (user, password) = UserBuilder.Build();

        var useCase = MakeUseCase(user);

        var request = new RequestLoginDto
        {
            Email = "invalidEmail@email.com",
            Password = password
        };
        
        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<InvalidLoginException>()
            .Where(exception => exception.Message.Equals(ResourceErrorMessages.INVALID_LOGIN));
    }
    
    [Fact]
    public async Task Login_Should_ReturnInvalidLoginException_WhenInvalidEmailAndPassword()
    {
        var (user, _) = UserBuilder.Build();

        var useCase = MakeUseCase(user);

        var request = new RequestLoginDto
        {
            Email = "invalidEmail@email.com",
            Password = "invalidPassword"
        };
        
        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<InvalidLoginException>()
            .Where(exception => exception.Message.Equals(ResourceErrorMessages.INVALID_LOGIN));
    }
    
    private LoginUseCase MakeUseCase(RecipeBook.Domain.Entities.User user) 
    {
        var readOnlyRepository = UserReadOnlyRepositoryBuilder
            .Instance()
            .RecoverWithEmailAndPassword(user)
            .Build();
        
        var passwordEncryptor = PasswordEncryptorBuilder
            .Instance();

        var tokenController = TokenControllerBuilder
            .Instance();

        return new LoginUseCase(readOnlyRepository, passwordEncryptor, tokenController);
    }
}