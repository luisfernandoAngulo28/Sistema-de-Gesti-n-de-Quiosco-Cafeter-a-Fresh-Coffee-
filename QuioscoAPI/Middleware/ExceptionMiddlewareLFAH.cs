using System.Net;
using System.Text.Json;

namespace QuioscoAPI.Middleware;

public class ExceptionMiddlewareLFAH
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddlewareLFAH> _logger;

    public ExceptionMiddlewareLFAH(RequestDelegate next, ILogger<ExceptionMiddlewareLFAH> logger)
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
            _logger.LogError(ex, "Error no controlado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "No autorizado"),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Ocurrió un error interno en el servidor")
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
