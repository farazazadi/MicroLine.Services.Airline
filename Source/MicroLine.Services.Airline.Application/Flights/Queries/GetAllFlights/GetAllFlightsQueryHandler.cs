using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Application.Flights.Queries.GetAllFlights;

internal class GetAllFlightsQueryHandler : IRequestHandler<GetAllFlightsQuery, IReadOnlyList<FlightDto>>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public GetAllFlightsQueryHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper
        )
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<FlightDto>> Handle(GetAllFlightsQuery query, CancellationToken token)
    {
        var flightList = await _airlineDbContext.Flights
            .AsNoTrackingWithIdentityResolution()
            .Include(flight => flight.OriginAirport)
            .Include(flight => flight.DestinationAirport)
            .Include(flight => flight.Aircraft)
            .Include(flight => flight.FlightCrewMembers)
            .Include(flight => flight.CabinCrewMembers)
            .ToListAsync(token);

        var flightDtoList = _mapper.Map<IReadOnlyList<FlightDto>>(flightList);

        return flightDtoList;
    }
}