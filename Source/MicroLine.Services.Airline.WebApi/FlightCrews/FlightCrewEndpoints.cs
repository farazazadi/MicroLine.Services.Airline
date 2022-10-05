using MediatR;
using MicroLine.Services.Airline.Application.FlightCrews.Commands.CreateFlightCrew;
using MicroLine.Services.Airline.Application.FlightCrews.Queries.GetAllFlightCrew;
using MicroLine.Services.Airline.Application.FlightCrews.Queries.GetFlightCrewById;

namespace MicroLine.Services.Airline.WebApi.FlightCrews;

internal static class FlightCrewEndpoints
{
    private const string BaseUrl = "api/flight-crew";

    public static WebApplication MapFlightCrewEndpoints(this WebApplication app)
    {
        app.MapPost(BaseUrl, CreateFlightCrewAsync);
        app.MapGet(BaseUrl + "/{id}", GetFlightCrewByIdAsync);
        app.MapGet(BaseUrl, GetAllFlightCrewAsync);

        return app;
    }

    private static async Task<IResult> CreateFlightCrewAsync(CreateFlightCrewCommand command,
        ISender sender, CancellationToken token)
    {
        var flightCrewDto = await sender.Send(command, token);

        return Results.Created($"{BaseUrl}/{flightCrewDto.Id}", flightCrewDto);
    }


    private static async Task<IResult> GetFlightCrewByIdAsync(Guid id, ISender sender, CancellationToken token)
    {
        var flightCrewDto = await sender.Send(new GetFlightCrewByIdQuery(id), token);

        return flightCrewDto is not null ? Results.Ok(flightCrewDto) : Results.NotFound();
    }

    private static async Task<IResult> GetAllFlightCrewAsync(ISender sender, CancellationToken token)
    {
        var flightCrewDtoList = await sender.Send(new GetAllFlightCrewQuery(), token);

        return Results.Ok(flightCrewDtoList);

    }
}
