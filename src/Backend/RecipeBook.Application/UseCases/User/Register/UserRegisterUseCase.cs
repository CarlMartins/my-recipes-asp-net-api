using AutoMapper;
using RecipeBook.Application.Services.Cryptography;
using RecipeBook.Application.Services.Token;
using RecipeBook.Application.UseCases.User.Register.Interfaces;
using RecipeBook.Comunication.Payloads;
using RecipeBook.Comunication.Responses;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.UserRepositories;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User.Register;

public class UserRegisterUseCase : IUserRegisterUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly ITokenController _tokenController;

    public UserRegisterUseCase(
        IUserWriteOnlyRepository userWriteOnlyRepository, 
        IMapper mapper, 
        IUnitOfWork unitOfWork, 
        IPasswordEncryptor passwordEncryptor, ITokenController tokenController)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _passwordEncryptor = passwordEncryptor;
        _tokenController = tokenController;
    }

    public async Task<SignedUpUserDto> Execute(SignUpUserRequestDto request)
    {
        Validate(request);

        var entity = _mapper.Map<Domain.Entities.User>(request);
        entity.Password = _passwordEncryptor.Encrypt(request.Password);

        await _userWriteOnlyRepository.Add(entity);
        await _unitOfWork.Commit();

        var token = _tokenController.GenerateToken(entity.Email);

        return new SignedUpUserDto
        {
            Token = token
        };
    }

    private void Validate(SignUpUserRequestDto request)
    {
        var validator = new UserSignUpValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ValidationErrorsException(errorMessages);
        }
    }
}