using FluentAssertions;
using RecipeBook.Application.UseCases.PasswordReset;
using RecipeBook.Comunication.DTOs.PasswordReset;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using TestHelpers.Encryptor;
using TestHelpers.Entities;
using TestHelpers.LoggedInUser;
using TestHelpers.Repositories;
using TestHelpers.Requests;

namespace UseCases.Test.User.PasswordReset;

public class PasswordResetUseCaseTest
{
    [Fact]
    public async Task PasswordReset_Should_ReturnSuccess()
    {
        var (user, password) = UserBuilder.Build();
        var useCase = MakeUseCase(user);
        
        var request = PasswordResetRequestBuilder.Build();
        request.CurrentPassword = password;
        
        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PasswordReset_Should_ReturnValidationException_WhenEmptyNewPassword()
    {
        var (user, password) = UserBuilder.Build();
        var useCase = MakeUseCase(user);

        Func<Task> act = async () => await useCase.Execute(new RequestPasswordResetDto
        {
            CurrentPassword = password,
            NewPassword = string.Empty
        });
        
        await act.Should().ThrowAsync<ValidationErrorsException>()
            .Where(exception => exception.Errors!.Count == 1)
            .Where(exception => exception.Errors!.Contains(ResourceErrorMessages.EMPTY_USER_PASSWORD));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task PasswordReset_Should_ReturnValidationException_WhenCurrentPasswordIsLessThanSixCaracters(
        int passwordSize)
    {
        var (user, password) = UserBuilder.Build();
        var useCase = MakeUseCase(user);
        
        var request = PasswordResetRequestBuilder.Build(passwordSize);
        request.CurrentPassword = password;
        
        Func<Task> act = async () => await useCase.Execute(request);
        
        await act.Should().ThrowAsync<ValidationErrorsException>()
            .Where(exception => exception.Errors!.Count == 1)
            .Where(exception => exception.Errors!.Contains(ResourceErrorMessages.MINIMUN_SIX_CHARACTERS_PASSWORD));
    }

    [Fact]
    public async Task PasswordReset_Should_ReturnValidationException_WhenInvalidCurrentPassword()
    {
        var (user, _) = UserBuilder.Build();
        var useCase = MakeUseCase(user);
        
        var request = PasswordResetRequestBuilder.Build();
        request.CurrentPassword = "InvalidPassword";
        
        Func<Task> act = async () => await useCase.Execute(request);
        
        await act.Should().ThrowAsync<ValidationErrorsException>()
            .Where(exception => exception.Errors!.Count == 1)
            .Where(exception => exception.Errors!.Contains(ResourceErrorMessages.INVALID_CURRENT_PASSWORD));
    }
    
    private PasswordResetUseCase MakeUseCase(RecipeBook.Domain.Entities.User user)
    {
        var passwordEncryptor = PasswordEncryptorBuilder.Instance();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();
        var updateOnlyRepository = UserUpdateOnlyRepositoryBuilder.Instance().GetById(user).Build();
        var loggedInUser = LoggedInUserBuilder.Instance().GetUser(user).Build();

        return new PasswordResetUseCase(updateOnlyRepository, loggedInUser, passwordEncryptor, unitOfWork);
    }
}