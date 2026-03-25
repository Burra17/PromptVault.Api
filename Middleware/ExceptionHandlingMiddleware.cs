using FluentValidation;
using System.Net;
using System.Text.Json;

namespace PromptVault.Api.Middleware;

// Global exception handler — catches all unhandled exceptions in the request pipeline
// and returns a consistent JSON error response instead of crashing with a 500 HTML page.
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Wraps every request in a try-catch. If no exception occurs, the request passes through normally.
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Pass the request to the next middleware/controller
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    // Maps exception types to HTTP status codes and writes a JSON error response.
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ValidationException => (int)HttpStatusCode.BadRequest,          // 400
            KeyNotFoundException => (int)HttpStatusCode.NotFound,            // 404
            _ => (int)HttpStatusCode.InternalServerError                    // 500
        };

        // Build the error response object
        var response = new
        {
            statusCode,
            message = exception.Message,
            // Include validation errors only for ValidationException
            errors = exception is ValidationException validationEx
                ? validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                : null
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}
