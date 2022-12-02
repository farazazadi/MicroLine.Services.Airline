using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Flights;

namespace MicroLine.Services.Airline.Application.Flights.Queries.GetAllFlights;

internal class GetAllFlightsQueryHandler : IRequestHandler<GetAllFlightsQuery, IReadOnlyList<FlightDto>>
{
    private readonly IFlightReadonlyRepository _flightReadonlyRepository;
    private readonly IMapper _mapper;

    public GetAllFlightsQueryHandler(
        IFlightReadonlyRepository flightReadonlyRepository,
        IMapper mapper)
    {
        _flightReadonlyRepository = flightReadonlyRepository;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<FlightDto>> Handle(GetAllFlightsQuery query, CancellationToken token)
    {
        var flightList = await _flightReadonlyRepository.GetAllAsync(token);

        var flightDtoList = _mapper.Map<IReadOnlyList<FlightDto>>(flightList);

        return flightDtoList;
    }
}