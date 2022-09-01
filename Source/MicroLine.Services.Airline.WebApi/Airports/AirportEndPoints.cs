using MediatR;
using MicroLine.Services.Airline.Application.Airports.Commands.CreateAirport;
using MicroLine.Services.Airline.Application.Airports.Queries.GetAirportById;

namespace MicroLine.Services.Airline.WebApi.Airports;

internal static class AirportEndpoints
{
    private const string BaseUrl = "api/airports";

    public static WebApplication MapAirportEndpoints(this WebApplication app)
    {
        app.MapPost(BaseUrl, CreateAirportAsync);
        app.MapGet(BaseUrl + "/{id}", GetAirportByIdAsync);

        return app;
    }

    private static async Task<IResult> CreateAirportAsync(CreateAirportCommand command,
        IMediator mediator,  CancellationToken token)
    {
        var airportDto = await mediator.Send(command, token);

        return Results.Created($"{BaseUrl}/{airportDto.Id}", airportDto);
    }

    private static async Task<IResult> GetAirportByIdAsync(Guid id,
        IMediator mediator, CancellationToken token)
    {
        var airportDto = await mediator.Send(new GetAirportByIdQuery(id), token);

        return airportDto is not null ? Results.Ok(airportDto) : Results.NotFound();
    }
}
