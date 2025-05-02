using AutoWrapper.Wrappers;
using FluentValidation;

namespace TestTaskAlreadyMedia.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception error)
        {
            throw error switch
            {
                ValidationException => HandleValidationException((ValidationException)error),
                _ => new ApiProblemDetailsException("Something went wrong.", StatusCodes.Status500InternalServerError),
            };
        }
        finally
        {
            if (!IsSuccessStatusCode(context.Response.StatusCode))
            {
                throw new ApiProblemDetailsException(context.Response.StatusCode);
            }
        }
    }

    private ApiProblemDetailsException HandleValidationException(ValidationException validationException)
    {
        var errorMessage = string.Join(Environment.NewLine, validationException.Errors.Select(error => error.ErrorMessage));

        if (string.IsNullOrWhiteSpace(errorMessage))
        {
            errorMessage = validationException.Message;
        }

        return new ApiProblemDetailsException(errorMessage, StatusCodes.Status500InternalServerError);
    }

    private bool IsSuccessStatusCode(int statusCode)
    {
        return statusCode >= StatusCodes.Status200OK && statusCode <= 299;
    }
}
