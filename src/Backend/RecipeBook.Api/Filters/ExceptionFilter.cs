using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RecipeBook.Comunication.Responses;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ExceptionBase)
        {
            HandleException(context);
        }
        else
        {
            HandleUnknownError(context);
        }
    }

    private void HandleException(ExceptionContext context)
    {
        if (context.Exception is ValidationErrorsException)
        {
            HandleValidationErrorException(context);
        }
    }

    private void HandleValidationErrorException(ExceptionContext context)
    {
        var validationErrorException = context.Exception as ValidationErrorsException;

        context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new ErrorResponseDto(validationErrorException!.Errors!));
    }
    
    private void HandleUnknownError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ErrorResponseDto(ResourceErrorMessages.UNKNOWN_ERROR));
    }
}