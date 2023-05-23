using System.Globalization;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using RecipeBook.Comunication.DTOs.Login;
using RecipeBook.Exceptions;

namespace WebApi.Test.V1;

public class ControllerBase : IClassFixture<WebAppFactory<Program>>
{
    private readonly HttpClient _client;
    
    public ControllerBase(WebAppFactory<Program> factory)
    {
        _client = factory.CreateClient();
        ResourceErrorMessages.Culture = CultureInfo.CurrentCulture;
    }

    protected async Task<HttpResponseMessage> PostRequest(string method, object body)
    {
        var jsonBody = JsonConvert.SerializeObject(body);
        
        return await _client.PostAsync(method, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
    }
    
    protected async Task<HttpResponseMessage> PutRequest(string method, object body, string token = "")
    {
        AuthorizeRequest(token);
        var jsonBody = JsonConvert.SerializeObject(body);
        
        return await _client.PutAsync(method, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
    }

    protected async Task<string> Login(string email, string password)
    {
        var request = new RequestLoginDto
        {
            Email = email,
            Password = password
        };
        
        var response = await PostRequest("login", request);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        return responseData.RootElement.GetProperty("token").GetString()!;
    }

    private void AuthorizeRequest(string token)
    {
        if (string.IsNullOrEmpty(token))
            return;
        
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }
}