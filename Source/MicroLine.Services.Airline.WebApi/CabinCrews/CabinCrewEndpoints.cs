using MediatR;
using MicroLine.Services.Airline.Application.CabinCrews.Commands.CreateCabinCrew;

namespace MicroLine.Services.Airline.WebApi.CabinCrews;

internal static class CabinCrewEndpoints
{
    private const string BaseUrl = "api/cabin-crew";

    public static WebApplication MapCabinCrewEndpoints(this WebApplication app)
    {
        app.Map(BaseUrl, CreateCabinCrewAsync);

        return app;
    }

    private static async Task<IResult> CreateCabinCrewAsync(CreateCabinCrewCommand command,
        ISender sender, CancellationToken token)
    {
        var cabinCrewDto = await sender.Send(command, token);

        return Results.Created($"{BaseUrl}/{cabinCrewDto.Id}", cabinCrewDto);
    }
}