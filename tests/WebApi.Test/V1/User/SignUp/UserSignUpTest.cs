using System.Net;
using System.Text.Json;
using FluentAssertions;
using RecipeBook.Exceptions;
using TestHelpers.Requests;

namespace WebApi.Test.V1.User.SignUp;

public class UserSignUpTest : ControllerBase
{
    private const string Method = "user";
    
    public UserSignUpTest(WebAppFactory<Program> factory) : base(factory)
    { 
    }

    [Fact]
    public async Task UserSignUp_Should_ReturnSuccess()
    {
        var request = UserSignUpRequestBuilder.Build();

        var response = await PostRequest(Method, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNull();
    }
    
    [Fact]
    public async Task UserSignUp_Should_ReturnEmptyNameError()
    {
        var request = UserSignUpRequestBuilder.Build();
        request.Name = string.Empty;

        var response = await PostRequest(Method, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        errors.Should().ContainSingle().And.Contain(x => x.GetString()!.Equals(ResourceErrorMessages.EMPTY_USER_NAME));
    }
}