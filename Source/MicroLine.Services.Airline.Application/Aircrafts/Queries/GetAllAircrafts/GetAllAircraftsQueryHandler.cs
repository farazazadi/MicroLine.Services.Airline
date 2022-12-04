using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Application.Aircrafts.Queries.GetAllAircrafts;

public class GetAllAircraftsQueryHandler : IRequestHandler<GetAllAircraftsQuery, IReadOnlyList<AircraftDto>>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public GetAllAircraftsQueryHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper)
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AircraftDto>> Handle(GetAllAircraftsQuery query, CancellationToken token)
    {
        var aircrafts = await _airlineDbContext.Aircrafts
            .AsNoTracking()
            .OrderByDescending(aircraft => aircraft.CreatedAtUtc)
            .ToListAsync(token);

        var aircraftDtoList = _mapper.Map<IReadOnlyList<AircraftDto>>(aircrafts);

        return aircraftDtoList;
    }
}