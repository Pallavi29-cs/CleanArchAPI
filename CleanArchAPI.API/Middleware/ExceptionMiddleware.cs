using System.Net;
using System.Text.Json;
using CleanArchAPI.Common.Models;

namespace CleanArchAPI.API.Middleware;

/// <summary>
/// Global Exception Middleware — catches ALL unhandled exceptions.
/// Returns consistent ApiResponse error format.
/// Logs every error automatically.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception. Path: {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ArgumentNullException       => (HttpStatusCode.BadRequest,          "A required argument was null."),
            ArgumentException           => (HttpStatusCode.BadRequest,          exception.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized,        "Unauthorized access."),
            KeyNotFoundException        => (HttpStatusCode.NotFound,            "Resource not found."),
            InvalidOperationException   => (HttpStatusCode.UnprocessableEntity, exception.Message),
            _                           => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = (int)statusCode;

        var response = ApiResponse<object>.Fail((int)statusCode, message);
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionMiddleware>();
}
