using RecipeBook.Application.Services.Cryptography;
using RecipeBook.Application.Services.Token;
using RecipeBook.Comunication.DTOs.Login;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Login.LogIn;

public class LoginUseCase : ILoginUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly ITokenController _tokenController;

    public LoginUseCase(
        IUserReadOnlyRepository readOnlyRepository, 
        IPasswordEncryptor passwordEncryptor, 
        ITokenController tokenController)
    {
        _readOnlyRepository = readOnlyRepository;
        _passwordEncryptor = passwordEncryptor;
        _tokenController = tokenController;
    }

    public async Task<ResponseLoginDto> Execute(RequestLoginDto request)
    {
        await Validate(request);
        
        var encryptedPassword = _passwordEncryptor.Encrypt(request.Password);
        
        var user = await _readOnlyRepository.RecoverWithEmailAndPassword(request.Email, encryptedPassword);

        if (user is null)
            throw new InvalidLoginException();
        
        return new ResponseLoginDto
        {
            Name = user.Name,
            Token = _tokenController.GenerateToken(user.Email)
        };
    }
    
    private async Task Validate(RequestLoginDto request)
    {
        var validator = new LoginValidator();
        var result = await validator.ValidateAsync(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ValidationErrorsException(errors);
        }
    }
}