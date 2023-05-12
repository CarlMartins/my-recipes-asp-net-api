using FluentAssertions;
using RecipeBook.Application.UseCases.PasswordReset;
using RecipeBook.Exceptions;
using TestHelpers.Requests;

namespace Validators.Test.PasswordReset;

public class PasswordResetValidatorTest
{
    [Fact]
    public void PasswordReset_Should_ReturnSuccess()
    {
        var validator = new PasswordResetValidator();

        var request = PasswordResetRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void PasswordReset_Should_ReturnError_WhenPasswordLengthIsLessThanSix(int passwordLength)
    {
        var validator = new PasswordResetValidator();

        var request = PasswordResetRequestBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.MINIMUN_SIX_CHARACTERS_PASSWORD));
    }
    
    [Fact]
    public void PasswordReset_Should_ReturnError_WhenPasswordIsEmpty()
    {
        var validator = new PasswordResetValidator();

        var request = PasswordResetRequestBuilder.Build();
        request.NewPassword = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_USER_PASSWORD));
    }
}