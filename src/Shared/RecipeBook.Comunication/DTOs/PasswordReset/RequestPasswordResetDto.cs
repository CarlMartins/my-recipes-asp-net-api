namespace RecipeBook.Comunication.DTOs.PasswordReset;

public class RequestPasswordResetDto
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}