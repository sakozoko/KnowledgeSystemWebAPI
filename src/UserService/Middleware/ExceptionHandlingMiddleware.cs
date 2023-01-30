using System.Text.Json;
using FluentValidation;
using UserService.Exceptions.IdentityResultFailedException;

namespace UserService.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var response = new
        {
            title = "Server Error",
            status = statusCode,
            detail = exception.Message,
            errors = GetErrors(exception)
        };
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            IdentityResultFailedException ex => GetIdentityResultFailedExceptionStatusCode(ex),
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static int GetIdentityResultFailedExceptionStatusCode(IdentityResultFailedException exception)
    {
        return exception.Code switch{
            IdentityResultFailedCodes.RoleNotFound => StatusCodes.Status404NotFound,
            IdentityResultFailedCodes.AccessDenied => StatusCodes.Status403Forbidden,
            IdentityResultFailedCodes.UserNotFound => StatusCodes.Status404NotFound,
            IdentityResultFailedCodes.BadUserModel => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError,
        };
    }

    private static IEnumerable<KeyValuePair<string, string[]>> GetErrors(Exception exception)
    {
        if (exception is ValidationException validationException)
            return validationException.Errors
                .Select(c =>
                {
                    return new KeyValuePair<string, string[]>(
                        c.PropertyName,
                        new[] { c.ErrorMessage });
                });
        if(exception is IdentityResultFailedException identityResultFailedException)
            return new KeyValuePair<string,string[]>[]
            {
                new(nameof(identityResultFailedException.Code), new[]{identityResultFailedException.Code!}),
            };
        return default!;
    }
}