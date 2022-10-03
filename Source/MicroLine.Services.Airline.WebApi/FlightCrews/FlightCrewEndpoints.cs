using MediatR;
using MicroLine.Services.Airline.Application.FlightCrews.Commands.CreateFlightCrew;

namespace MicroLine.Services.Airline.WebApi.FlightCrews;

internal static class FlightCrewEndpoints
{
    private const string BaseUrl = "api/flight-crew";

    public static WebApplication MapFlightCrewEndpoints(this WebApplication app)
    {
        app.MapPost(BaseUrl, CreateFlightCrewAsync);
        return app;
    }

    private static async Task<IResult> CreateFlightCrewAsync(CreateFlightCrewCommand command,
        ISender sender, CancellationToken token)
    {
        var flightCrewDto = await sender.Send(command, token);

        return Results.Created($"{BaseUrl}/{flightCrewDto.Id}", flightCrewDto);
    }
}
