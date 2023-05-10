using System.Net;
using System.Text.Json;
using FluentAssertions;
using RecipeBook.Comunication.DTOs.Login;
using RecipeBook.Exceptions;

namespace WebApi.Test.V1.User.Login.LogIn;

public class LoginTest : ControllerBase
{
    private const string Method = "login";
    
    private RecipeBook.Domain.Entities.User _user;
    private string _password;
    
    public LoginTest(WebAppFactory<Program> factory) : base(factory)
    {
        _user = factory.GetUser();
        _password = factory.GetPassword();
    }

    [Fact]
    public async Task Login_Should_ReturnSuccess()
    {
        var request = new RequestLoginDto
        {
            Email = _user.Email,
            Password = _password,
        };

        var response = await PostRequest(Method, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace()
            .And.Be(_user.Name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Login_Should_Return401Error_WhenInvalidPassword()
    {
        var request = new RequestLoginDto
        {
            Email = _user.Email,
            Password = "invalidPassword",
        };

        var response = await PostRequest(Method, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        var errors = responseData.RootElement.GetProperty("errors").Deserialize<List<string>>();
        errors.Should().ContainSingle().And.ContainSingle(ResourceErrorMessages.INVALID_LOGIN);
    }
    
    [Fact]
    public async Task Login_Should_Return401Error_WhenInvalidEmail()
    {
        var request = new RequestLoginDto
        {
            Email = "invalidEmail@email.com",
            Password = _password,
        };

        var response = await PostRequest(Method, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        var errors = responseData.RootElement.GetProperty("errors").Deserialize<List<string>>();
        errors.Should().ContainSingle().And.ContainSingle(ResourceErrorMessages.INVALID_LOGIN);
    }
    
    [Fact]
    public async Task Login_Should_Return401Error_WhenInvalidRequest()
    {
        var request = new RequestLoginDto
        {
            Email = "invalidEmail@email.com",
            Password = "invalidPassword",
        };

        var response = await PostRequest(Method, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        var errors = responseData.RootElement.GetProperty("errors").Deserialize<List<string>>();
        errors.Should().ContainSingle().And.ContainSingle(ResourceErrorMessages.INVALID_LOGIN);
    }
}