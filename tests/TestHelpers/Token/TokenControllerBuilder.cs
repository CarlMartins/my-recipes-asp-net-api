using Moq;
using RecipeBook.Application.Services.Token;

namespace TestHelpers.Token;

public class TokenControllerBuilder
{
    private readonly Mock<ITokenController> _mock;
    
    private TokenControllerBuilder()
    {
        _mock ??= new Mock<ITokenController>();
    }

    public static ITokenController Instance()
    {
        return new TokenController(1000.0, "bnk5YyFkUmNRUEQ3WnpPOWhRJTU4Tzc3JGZh");
    }
}