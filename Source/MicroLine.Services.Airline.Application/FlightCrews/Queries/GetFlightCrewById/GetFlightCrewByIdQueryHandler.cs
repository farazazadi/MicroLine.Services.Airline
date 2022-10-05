using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;

namespace MicroLine.Services.Airline.Application.FlightCrews.Queries.GetFlightCrewById;

internal class GetFlightCrewByIdQueryHandler : IRequestHandler<GetFlightCrewByIdQuery, FlightCrewDto>
{
    private readonly IFlightCrewReadonlyRepository _flightCrewReadonlyRepository;
    private readonly IMapper _mapper;

    public GetFlightCrewByIdQueryHandler(
        IFlightCrewReadonlyRepository flightCrewReadonlyRepository,
        IMapper mapper
        )
    {
        _flightCrewReadonlyRepository = flightCrewReadonlyRepository;
        _mapper = mapper;
    }

    public async Task<FlightCrewDto> Handle(GetFlightCrewByIdQuery query, CancellationToken token)
    {
        var flightCrew = await _flightCrewReadonlyRepository.GetAsync(query.Id, token);

        var flightCrewDto = _mapper.Map<FlightCrewDto>(flightCrew);

        return flightCrewDto;
    }
}