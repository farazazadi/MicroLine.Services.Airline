using MediatR;
using MicroLine.Services.Airline.Application.Flights.Commands.ScheduleFlight;
using MicroLine.Services.Airline.Application.Flights.Queries.GetFlightById;

namespace MicroLine.Services.Airline.WebApi.Flights;

internal static class FlightEndpoints
{
    private const string BaseUrl = "api/flights";

    public static WebApplication MapFlightEndpoints(this WebApplication app)
    {
        app.MapPost(BaseUrl, ScheduleFlightAsync);
        app.MapGet(BaseUrl + "/{id}", GetFlightByIdAsync);
        return app;
    }

    private static async Task<IResult> ScheduleFlightAsync(ScheduleFlightCommand command,
        ISender sender, CancellationToken token)
    {
        var flightDto = await sender.Send(command, token);

        return Results.Created($"{BaseUrl}/{flightDto.Id}", flightDto);
    }

    private static async Task<IResult> GetFlightByIdAsync(Guid id, ISender sender, CancellationToken token)
    {
        var flightDto = await sender.Send(new GetFlightByIdQuery(id), token);

        return flightDto is not null ? Results.Ok(flightDto) : Results.NotFound();
    }
}