namespace RecipeBook.Comunication.Payloads;

public class SignUpUserRequestDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Contact { get; set; } = null!;
}