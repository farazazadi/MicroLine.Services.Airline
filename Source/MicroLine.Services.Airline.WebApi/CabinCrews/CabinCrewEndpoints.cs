using MediatR;
using MicroLine.Services.Airline.Application.CabinCrews.Commands.CreateCabinCrew;
using MicroLine.Services.Airline.Application.CabinCrews.Queries.GetAllCabinCrew;
using MicroLine.Services.Airline.Application.CabinCrews.Queries.GetCabinCrewById;

namespace MicroLine.Services.Airline.WebApi.CabinCrews;

internal static class CabinCrewEndpoints
{
    private const string BaseUrl = "api/cabin-crew";

    public static WebApplication MapCabinCrewEndpoints(this WebApplication app)
    {
        app.MapPost(BaseUrl, CreateCabinCrewAsync);
        app.MapGet(BaseUrl + "/{id}", GetCabinCrewByIdAsync);
        app.MapGet(BaseUrl, GetAllCabinCrewAsync);

        return app;
    }


    private static async Task<IResult> CreateCabinCrewAsync(CreateCabinCrewCommand command,
        ISender sender, CancellationToken token)
    {
        var cabinCrewDto = await sender.Send(command, token);

        return Results.Created($"{BaseUrl}/{cabinCrewDto.Id}", cabinCrewDto);
    }

    private static async Task<IResult> GetCabinCrewByIdAsync(Guid id, ISender sender, CancellationToken token)
    {
        var cabinCrewDto = await sender.Send(new GetCabinCrewByIdQuery(id), token);

        return Results.Ok(cabinCrewDto);
    }

    private static async Task<IResult> GetAllCabinCrewAsync(ISender sender, CancellationToken token)
    {
        var cabinCrewDtoList = await sender.Send(new GetAllCabinCrewQuery(), token);

        return Results.Ok(cabinCrewDtoList);
    }

}