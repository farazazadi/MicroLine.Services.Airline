using MediatR;
using MicroLine.Services.Airline.Application.Airports.Commands.CreateAirport;

namespace MicroLine.Services.Airline.WebApi.Airports;

internal static class AirportEndpoints
{
    private const string AirportsEndpointsBaseUrl = "api/airports";

    public static WebApplication MapAirportEndpoints(this WebApplication app)
    {
        app.MapPost(AirportsEndpointsBaseUrl, CreateAirportAsync);

        return app;
    }

    private static async Task<IResult> CreateAirportAsync(CreateAirportCommand command,
        IMediator mediator,  CancellationToken token)
    {
        var airportDto = await mediator.Send(command, token);

        return Results.Created($"{AirportsEndpointsBaseUrl}/{airportDto.Id}", airportDto);
    }
}
