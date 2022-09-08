using MediatR;
using MicroLine.Services.Airline.Application.Aircrafts.Commands.CreateAircraft;
using MicroLine.Services.Airline.Application.Aircrafts.Queries.GetAircraftById;
using MicroLine.Services.Airline.Application.Aircrafts.Queries.GetAllAircrafts;

namespace MicroLine.Services.Airline.WebApi.Aircrafts;

internal static class AircraftEndpoints
{
    private const string BaseUrl = "api/aircrafts";

    public static WebApplication MapAircraftEndpoints(this WebApplication app)
    {
        app.MapPost(BaseUrl, CreateAircraftAsync);
        app.MapGet(BaseUrl + "/{id}", GetAircraftByIdAsync);
        app.MapGet(BaseUrl, GetAllAircraftsAsync);

        return app;
    }

    private static async Task<IResult> CreateAircraftAsync(CreateAircraftCommand command,
        ISender sender, CancellationToken token)
    {
        var aircraftDto = await sender.Send(command, token);

        return Results.Created($"{BaseUrl}/{aircraftDto.Id}", aircraftDto);
    }

    private static async Task<IResult> GetAircraftByIdAsync(Guid id,
        ISender sender, CancellationToken token)
    {
        var aircraftDto = await sender.Send(new GetAircraftByIdQuery(id), token);

        return aircraftDto is not null ? Results.Ok(aircraftDto) : Results.NotFound();
    }

    private static async Task<IResult> GetAllAircraftsAsync(ISender sender, CancellationToken token)
    {
        var aircrafts = await sender.Send(new GetAllAircraftsQuery(), token);

        return Results.Ok(aircrafts);
    }

}
