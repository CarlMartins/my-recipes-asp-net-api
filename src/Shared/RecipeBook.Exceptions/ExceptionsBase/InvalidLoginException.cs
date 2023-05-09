namespace RecipeBook.Exceptions.ExceptionsBase;

public class InvalidLoginException : ExceptionBase
{
    public InvalidLoginException() : base(ResourceErrorMessages.INVALID_LOGIN)
    {
        
    }
}