using MicroLine.Services.Airline.Application.Common.Exceptions;
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
        catch (ApplicationExceptionBase exception)
        {
            await HandleApplicationExceptionAsync(context, exception);
        }
        catch (Exception exception)
        {
            await HandleUnexpectedExceptionsAsync(context, exception, logger);
        }
    }

    private static async Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
    {
        await ReturnProblemResponseAsync(
            exception.Code,
            exception.Message,
            StatusCodes.Status400BadRequest,
            context);
    }


    private static async Task HandleApplicationExceptionAsync(HttpContext context, ApplicationExceptionBase exception)
    {

        await ReturnProblemResponseAsync(
            exception.Code,
            exception.Message,
            StatusCodes.Status400BadRequest,
            context);
    }


    private static async Task ReturnProblemResponseAsync(
        string exceptionCode,
        string exceptionMessage,
        int statusCode,
        HttpContext context)
    {
        var extensions = new Dictionary<string, object>
        {
            {"exceptionCode", exceptionCode}
        };

        await Results.Problem(
                detail: exceptionMessage,
                statusCode: statusCode,
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