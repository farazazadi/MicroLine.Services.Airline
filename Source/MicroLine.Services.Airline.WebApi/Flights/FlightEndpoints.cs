using MediatR;
using MicroLine.Services.Airline.Application.Flights.Commands.ScheduleFlight;

namespace MicroLine.Services.Airline.WebApi.Flights;

internal static class FlightEndpoints
{
    private const string BaseUrl = "api/flights";

    public static WebApplication MapFlightEndpoints(this WebApplication app)
    {
        app.MapPost(BaseUrl, ScheduleFlightAsync);
        return app;
    }

    private static async Task<IResult> ScheduleFlightAsync(ScheduleFlightCommand command,
        ISender sender, CancellationToken token)
    {
        var flightDto = await sender.Send(command, token);

        return Results.Created($"{BaseUrl}/{flightDto.Id}", flightDto);
    }
}