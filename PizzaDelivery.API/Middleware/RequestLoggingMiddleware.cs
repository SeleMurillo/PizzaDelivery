using System.Diagnostics;

namespace PizzaDelivery.API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
            stopwatch.Stop();

            var statusCode = context.Response.StatusCode;
            var logLevel = statusCode >= 500 ? LogLevel.Error : 
                           statusCode >= 400 ? LogLevel.Warning : 
                           LogLevel.Information;

            _logger.Log(logLevel,
                "HTTP {Method} {Path} respondió {StatusCode} en {ElapsedMs}ms | User: {User}",
                context.Request.Method,
                context.Request.Path,
                statusCode,
                stopwatch.ElapsedMilliseconds,
                context.User.Identity?.Name ?? "anonymous");
        }
        catch (Exception)
        {
            stopwatch.Stop();
            throw;
        }
    }
}