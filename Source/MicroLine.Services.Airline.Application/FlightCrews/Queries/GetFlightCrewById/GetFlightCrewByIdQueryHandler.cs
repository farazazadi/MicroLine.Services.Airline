using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Application.Common.Exceptions;
using MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;

namespace MicroLine.Services.Airline.Application.FlightCrews.Queries.GetFlightCrewById;

internal class GetFlightCrewByIdQueryHandler : IRequestHandler<GetFlightCrewByIdQuery, FlightCrewDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public GetFlightCrewByIdQueryHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper
        )
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }

    public async Task<FlightCrewDto> Handle(GetFlightCrewByIdQuery query, CancellationToken token)
    {
        var flightCrew = await _airlineDbContext.FlightCrews
            .FindAsync(new object[] { (Id)query.Id }, token);

        return _mapper.Map<FlightCrewDto>(flightCrew ?? throw new NotFoundException("FlightCrew", query.Id));
    }
}