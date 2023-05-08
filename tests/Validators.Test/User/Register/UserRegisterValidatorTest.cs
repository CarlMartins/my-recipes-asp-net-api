using FluentAssertions;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Exceptions;
using TestHelpers.Requests;

namespace Validators.Test.User.Register;

public class UserRegisterValidatorTest
{
    [Fact]
    public void Validate_Success_UserSignUp()
    {
        var validator = new UserSignUpValidator();

        var request = UserSignUpRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void Validate_Error_EmptyUsername()
    {
        var validator = new UserSignUpValidator();

        var request = UserSignUpRequestBuilder.Build();
        request.Name = String.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_USER_NAME));
    }
    
    [Fact]
    public void Validate_Error_EmptyEmail()
    {
        var validator = new UserSignUpValidator();

        var request = UserSignUpRequestBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_USER_EMAIL));
    }

    [Fact]
    public void Validate_Error_EmptyPassword()
    {
        var validator = new UserSignUpValidator();

        var request = UserSignUpRequestBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_USER_PASSWORD));
    }
    
    [Fact]
    public void Validate_Error_EmptyContact()
    {
        var validator = new UserSignUpValidator();

        var request = UserSignUpRequestBuilder.Build();
        request.Contact = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_USER_CONTACT));
    }
    
    [Fact]
    public void Validate_Error_InvalidEmail()
    {
        var validator = new UserSignUpValidator();

        var request = UserSignUpRequestBuilder.Build();
        request.Email = "invalid_email";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.INVALID_USER_EMAIL));
    }
    
    [Fact]
    public void Validate_Error_InvalidContact()
    {
        var validator = new UserSignUpValidator();

        var request = UserSignUpRequestBuilder.Build();
        request.Contact = "invalid_contact";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.INVALID_USER_CONTACT));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validate_Error_InvalidPasswordSize(int passwordSize)
    {
        var validator = new UserSignUpValidator();

        var request = UserSignUpRequestBuilder.Build(passwordSize);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.MINIMUN_SIX_CHARACTERS_PASSWORD));
    }
}