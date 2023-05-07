using System.Globalization;
using System.Text;
using Newtonsoft.Json;
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
}