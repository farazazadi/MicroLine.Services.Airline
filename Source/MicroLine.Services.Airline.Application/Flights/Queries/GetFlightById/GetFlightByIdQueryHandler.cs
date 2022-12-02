using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Flights;

namespace MicroLine.Services.Airline.Application.Flights.Queries.GetFlightById;

internal class GetFlightByIdQueryHandler : IRequestHandler<GetFlightByIdQuery, FlightDto>
{
    private readonly IFlightReadonlyRepository _flightReadonlyRepository;
    private readonly IMapper _mapper;

    public GetFlightByIdQueryHandler(
        IFlightReadonlyRepository flightReadonlyRepository,
        IMapper mapper
        )
    {
        _flightReadonlyRepository = flightReadonlyRepository;
        _mapper = mapper;
    }
    public async Task<FlightDto> Handle(GetFlightByIdQuery query, CancellationToken token)
    {
        var flight = await _flightReadonlyRepository.GetAsync(flight => flight.Id == query.Id, token);

        var flightDto = _mapper.Map<FlightDto>(flight);

        return flightDto;
    }
}