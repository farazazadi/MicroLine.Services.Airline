using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Application.FlightCrews.Queries.GetAllFlightCrew;

internal class GetAllFlightCrewQueryHandler : IRequestHandler<GetAllFlightCrewQuery, IReadOnlyList<FlightCrewDto>>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public GetAllFlightCrewQueryHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper)
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<FlightCrewDto>> Handle(GetAllFlightCrewQuery query, CancellationToken token)
    {
        var flightCrewList = await _airlineDbContext.FlightCrews
            .AsNoTracking()
            .OrderByDescending(flightCrew => flightCrew.CreatedAtUtc)
            .ToListAsync(token);

        var flightCrewDtoList = _mapper.Map<IReadOnlyList<FlightCrewDto>>(flightCrewList);

        return flightCrewDtoList;
    }
}