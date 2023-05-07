using FluentAssertions;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using TestHelpers.Encryptor;
using TestHelpers.Mapper;
using TestHelpers.Repositories;
using TestHelpers.Requests;
using TestHelpers.Token;

namespace UseCases.Test.User.SignUp;

public class UserSignUpUseCaseTest
{
    [Fact]
    public async Task SignUpUser_Should_ReturnSuccess()
    {
        var request = UserSignUpRequestBuilder.Build();
        
        var useCase = MakeUseCase();

        var response = await useCase.Execute(request);

        response.Should().NotBeNull();
        response.Token.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task SignUpUser_Should_ReturnValidationException_WhenAlreadyExistsUserWithEmail()
    {
        var request = UserSignUpRequestBuilder.Build();
        
        var useCase = MakeUseCase(request.Email);
        
        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ValidationErrorsException>()
            .Where(exception => exception.Errors!.Count == 1)
            .Where(exception => exception.Errors!.Contains(ResourceErrorMessages.EMAIL_ALREADY_EXISTS));
    }
    
    [Fact]
    public async Task SignUpUser_Should_ReturnValidationException_WhenEmptyEmail()
    {
        var request = UserSignUpRequestBuilder.Build();
        request.Email = string.Empty;
        
        var useCase = MakeUseCase();
        
        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ValidationErrorsException>()
            .Where(exception => exception.Errors!.Count == 1)
            .Where(exception => exception.Errors!.Contains(ResourceErrorMessages.EMPTY_USER_EMAIL));
    }
    
    private static UserRegisterUseCase MakeUseCase(string email = "") 
    {
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder
            .Instance()
            .Build();
        
        var mapper = MapperBuilder
            .Instance();   
        
        var unitOfWork = UnitOfWorkBuilder
            .Instance()
            .Build();

        var encryptor = PasswordEncryptorBuilder
            .Instance();
        
        var tokenController = TokenControllerBuilder
            .Instance();
        
        var readOnlyRepository = UserReadOnlyRepositoryBuilder
            .Instance()
            .AlreadyExistsUserWithEmail(email)
            .Build();

        return new UserRegisterUseCase(
            writeOnlyRepository, mapper, unitOfWork, encryptor, tokenController, readOnlyRepository);
    }
    
}