using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.WebApi.Common.Middleware;

internal class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException exception)
        {
            await HandleDomainExceptionAsync(context, exception);
        }
        catch (Exception exception)
        {
            await HandleUnexpectedExceptionsAsync(context, exception, logger);
        }
    }

    private static async Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
    {

        var extensions = new Dictionary<string, object>
        {
            {"exceptionCode", exception.Code}
        };

        await Results.Problem(
                detail: exception.Message,
                statusCode: StatusCodes.Status400BadRequest,
                instance: context.Request.Path,
                extensions: extensions
                )
            .ExecuteAsync(context);
    }


    private static async Task HandleUnexpectedExceptionsAsync(HttpContext context, Exception exception,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        await Results.Problem(
                detail: exception.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                instance: context.Request.Path
            )
            .ExecuteAsync(context);

        logger.LogError(exception, exception.Message);
    }
}