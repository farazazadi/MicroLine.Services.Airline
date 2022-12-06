using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Application.Flights.Queries.GetFlightById;

internal class GetFlightByIdQueryHandler : IRequestHandler<GetFlightByIdQuery, FlightDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public GetFlightByIdQueryHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper
        )
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }
    public async Task<FlightDto> Handle(GetFlightByIdQuery query, CancellationToken token)
    {
        var flight = await _airlineDbContext.Flights
                .AsNoTracking()
                .Include(flight => flight.OriginAirport)
                .Include(flight => flight.DestinationAirport)
                .Include(flight => flight.Aircraft)
                .Include(flight => flight.FlightCrewMembers)
                .Include(flight => flight.CabinCrewMembers)
                .FirstOrDefaultAsync(flight => flight.Id == query.Id, token);


        var flightDto = _mapper.Map<FlightDto>(flight);

        return flightDto;
    }
}