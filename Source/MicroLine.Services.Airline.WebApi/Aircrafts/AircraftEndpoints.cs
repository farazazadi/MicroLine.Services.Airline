using MediatR;
using MicroLine.Services.Airline.Application.Aircrafts.Commands.CreateAircraft;

namespace MicroLine.Services.Airline.WebApi.Aircrafts;

internal static class AircraftEndpoints
{
    private const string BaseUrl = "api/aircrafts";

    public static WebApplication MapAircraftEndpoints(this WebApplication app)
    {
        app.MapPost(BaseUrl, CreateAircraftAsync);

        return app;
    }

    private static async Task<IResult> CreateAircraftAsync(CreateAircraftCommand command,
        ISender sender, CancellationToken token)
    {
        var aircraftDto = await sender.Send(command, token);

        return Results.Created($"{BaseUrl}/{aircraftDto.Id}", aircraftDto);
    }
}
