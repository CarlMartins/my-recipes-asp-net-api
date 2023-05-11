using System.Reflection;
using FluentValidation.Results;
using RecipeBook.Application.Services.Cryptography;
using RecipeBook.Application.Services.LoggedInUser;
using RecipeBook.Comunication.DTOs.PasswordReset;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.PasswordReset;

public class PasswordResetUseCase : IPasswordResetUseCase
{
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    private readonly ILoggedInUser _loggedInUser;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly IUnitOfWork _unitOfWork;

    public PasswordResetUseCase(
        IUserUpdateOnlyRepository updateOnlyRepository,
        ILoggedInUser loggedInUser,
        IPasswordEncryptor passwordEncryptor, 
        IUnitOfWork unitOfWork)
    {
        _updateOnlyRepository = updateOnlyRepository;
        _loggedInUser = loggedInUser;
        _passwordEncryptor = passwordEncryptor;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestPasswordResetDto request)
    {
        var currentUser = await _loggedInUser.GetUser();

        var user = await _updateOnlyRepository.GetById(currentUser!.Id);

        Validate(request, currentUser);

        user!.Password = _passwordEncryptor.Encrypt(request.NewPassword);
        
        await _unitOfWork.Commit();
    }

    private void Validate(RequestPasswordResetDto request, Domain.Entities.User user)
    {
        var validator = new PasswordResetValidator();
        var result = validator.Validate(request);

        var currentPassword = _passwordEncryptor.Encrypt(request.CurrentPassword);

        if (!user.Password.Equals(currentPassword))
        {
            result.Errors.Add(new ValidationFailure("CurrentPassword", ResourceErrorMessages.INVALID_CURRENT_PASSWORD));
        }

        if (result.IsValid)
            return;
        
        var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
        throw new ValidationErrorsException(errors);
    }
}