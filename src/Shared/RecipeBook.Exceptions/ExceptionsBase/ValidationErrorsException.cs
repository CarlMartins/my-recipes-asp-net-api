namespace RecipeBook.Exceptions.ExceptionsBase;

public class ValidationErrorsException : ExceptionBase
{
    public List<string>? Errors { get; set; }

    public ValidationErrorsException(List<string> errorMessages)
    {
        Errors = errorMessages;
    }
}