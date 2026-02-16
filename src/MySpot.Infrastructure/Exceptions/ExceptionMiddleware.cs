using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MySpot.Core.Exceptions;

namespace MySpot.Infrastructure.Exceptions;

internal sealed class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            logger.LogError(exception, exception.Message);
            await HandleExceptionAsync(exception, context);
        }
    }

    private async Task HandleExceptionAsync(
        Exception exception, HttpContext context)
    {
        var (statusCode, error) = exception switch
        {
            MySpotException => (StatusCodes.Status400BadRequest,
                new Error(
                    exception.GetType().Name.Replace("Exception", string.Empty).Underscore(),
                    exception.Message)),
            _ => (StatusCodes.Status500InternalServerError,
                new Error("Error", "Internal server error")),
        };
        
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(error);
    }

    private record Error(string Code, string Reason);
}
