using MediatR;
using MicroLine.Services.Airline.Application.Flights.Commands.ScheduleFlight;
using MicroLine.Services.Airline.Application.Flights.Queries.GetAllFlights;
using MicroLine.Services.Airline.Application.Flights.Queries.GetFlightById;

namespace MicroLine.Services.Airline.WebApi.Flights;

internal static class FlightEndpoints
{
    private const string BaseUrl = "api/flights";

    public static WebApplication MapFlightEndpoints(this WebApplication app)
    {
        app.MapPost(BaseUrl, ScheduleFlightAsync);
        app.MapGet(BaseUrl + "/{id}", GetFlightByIdAsync);
        app.MapGet(BaseUrl, GetAllFlightsAsync);
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

        return Results.Ok(flightDto);
    }

    private static async Task<IResult> GetAllFlightsAsync(ISender sender, CancellationToken token)
    {
        var flightDtoList = await sender.Send(new GetAllFlightsQuery(), token);

        return Results.Ok(flightDtoList);
    }
}