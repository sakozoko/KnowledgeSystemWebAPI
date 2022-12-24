using System.Text.Json;
using FluentValidation;

namespace WebApi;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;
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
    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            BadHttpRequestException => StatusCodes.Status400BadRequest,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            //write status code for 404 and 500
            
            
            _ => StatusCodes.Status500InternalServerError
        };

    private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
    {
        IReadOnlyDictionary<string, string[]> errors = null;
        if (exception is ValidationException validationException)
        {
            errors = validationException.Errors.ToDictionary(
                keySelector: error => error.PropertyName,
                elementSelector: error => new[] { error.ErrorMessage });
        }
        return errors;
    }
}