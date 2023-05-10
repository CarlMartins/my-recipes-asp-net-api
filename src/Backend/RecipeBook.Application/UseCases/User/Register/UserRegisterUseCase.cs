using AutoMapper;
using FluentValidation.Results;
using RecipeBook.Application.Services.Cryptography;
using RecipeBook.Application.Services.Token;
using RecipeBook.Application.UseCases.User.Register.Interfaces;
using RecipeBook.Comunication.DTOs.SignUp;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User.Register;

public class UserRegisterUseCase : IUserRegisterUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly ITokenController _tokenController;

    public UserRegisterUseCase(
        IUserWriteOnlyRepository userWriteOnlyRepository, 
        IMapper mapper, 
        IUnitOfWork unitOfWork, 
        IPasswordEncryptor passwordEncryptor, 
        ITokenController tokenController, 
        IUserReadOnlyRepository userReadOnlyRepository)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _passwordEncryptor = passwordEncryptor;
        _tokenController = tokenController;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<ResponseSignedUpUserDto> Execute(SignUpUserRequestDto request)
    {
        await Validate(request);

        var entity = _mapper.Map<Domain.Entities.User>(request);
        entity.Password = _passwordEncryptor.Encrypt(request.Password);

        await _userWriteOnlyRepository.Add(entity);
        await _unitOfWork.Commit();

        var token = _tokenController.GenerateToken(entity.Email);

        return new ResponseSignedUpUserDto
        {
            Token = token
        };
    }

    private async Task Validate(SignUpUserRequestDto request)
    {
        var validator = new UserSignUpValidator();
        var result = await validator.ValidateAsync(request);

        var emailAlreadyExists = await _userReadOnlyRepository.AlreadyExistsUserWithEmail(request.Email);

        if (emailAlreadyExists)
        {
            result.Errors.Add(new ValidationFailure("email", ResourceErrorMessages.EMAIL_ALREADY_EXISTS));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ValidationErrorsException(errorMessages);
        }
    }
}