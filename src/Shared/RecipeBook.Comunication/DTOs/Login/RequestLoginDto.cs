namespace RecipeBook.Comunication.DTOs.Login;

public class RequestLoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}