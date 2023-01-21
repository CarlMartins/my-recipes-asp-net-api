namespace RecipeBook.Comunication.Responses;

public class ErrorResponseDto
{
    public List<string> Errors { get; set; }

    public ErrorResponseDto(string errorMessage)
    {
        Errors = new List<string>
        {
            errorMessage
        };
    }

    public ErrorResponseDto(List<string> errorMessages)
    {
        Errors = errorMessages;
    }
}