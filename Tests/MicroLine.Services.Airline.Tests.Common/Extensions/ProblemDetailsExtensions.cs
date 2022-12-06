using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace MicroLine.Services.Airline.Tests.Common.Extensions;
public static class ProblemDetailsExtensions
{
    public static string ExceptionCode => "exceptionCode";

    public static async Task<ProblemDetails> GetProblemResultAsync(this HttpResponseMessage httpResponseMessage)
    {
        var mediaType = httpResponseMessage.Content.Headers.ContentType?.MediaType;

        if (mediaType is null ||
            !mediaType.Equals("application/problem+json", StringComparison.InvariantCultureIgnoreCase))
            return new ProblemDetails();

        var problemDetails = await httpResponseMessage.Content.ReadFromJsonAsync<ProblemDetails>() ?? new ProblemDetails();

        return problemDetails;
    }
}
