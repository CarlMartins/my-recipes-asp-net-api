namespace RecipeBook.Exceptions.ExceptionsBase;

public class ExceptionBase : SystemException
{
    protected ExceptionBase()
    { }
    
    protected ExceptionBase(string message) : base(message)
    { }
}