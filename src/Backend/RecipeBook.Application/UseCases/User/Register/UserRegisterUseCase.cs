using AutoMapper;
using RecipeBook.Application.UseCases.User.Register.Interfaces;
using RecipeBook.Comunication.Payloads;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.UserRepositories;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User.Register;

public class UserRegisterUseCase : IUserRegisterUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserRegisterUseCase(IUserWriteOnlyRepository userWriteOnlyRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(SignUpUserRequestDto request)
    {
        Validate(request);

        var entity = _mapper.Map<Domain.Entities.User>(request);
        entity.Password = "crypt";

        await _userWriteOnlyRepository.Add(entity);
        await _unitOfWork.Commit();
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