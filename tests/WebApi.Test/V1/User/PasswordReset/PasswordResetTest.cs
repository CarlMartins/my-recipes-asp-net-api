using System.Net;
using System.Text.Json;
using FluentAssertions;
using RecipeBook.Exceptions;
using TestHelpers.Requests;

namespace WebApi.Test.V1.User.PasswordReset;

public class PasswordResetTest : ControllerBase
{
    private const string Method = "user/password-reset";
    
    private readonly RecipeBook.Domain.Entities.User _user;
    private readonly string _password;
    
    public PasswordResetTest(WebAppFactory<Program> factory) 
        : base(factory)
    {
        _password = factory.GetPassword();
        _user = factory.GetUser();
    }
    
    [Fact]
    public async Task PasswordReset_Should_ReturnSuccess()
    {
        var token = await Login(_user.Email, _password);
        var request = PasswordResetRequestBuilder.Build();
        
        request.CurrentPassword = _password;

        var response = await PutRequest(Method, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task PasswordReset_Should_ReturnValidationException_WhenEmptyPassword()
    {
        var token = await Login(_user.Email, _password);
        var request = PasswordResetRequestBuilder.Build();
        
        request.CurrentPassword = _password;
        request.NewPassword = string.Empty;

        var response = await PutRequest(Method, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        errors.Should().ContainSingle().And
            .Contain(x => x.GetString()!.Equals(ResourceErrorMessages.EMPTY_USER_PASSWORD));
    }
}