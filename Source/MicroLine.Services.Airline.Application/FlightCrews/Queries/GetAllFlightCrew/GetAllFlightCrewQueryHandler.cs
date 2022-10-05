using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;

namespace MicroLine.Services.Airline.Application.FlightCrews.Queries.GetAllFlightCrew;

internal class GetAllFlightCrewQueryHandler : IRequestHandler<GetAllFlightCrewQuery, IReadOnlyList<FlightCrewDto>>
{
    private readonly IFlightCrewReadonlyRepository _flightCrewReadonlyRepository;
    private readonly IMapper _mapper;

    public GetAllFlightCrewQueryHandler(
        IFlightCrewReadonlyRepository flightCrewReadonlyRepository,
        IMapper mapper)
    {
        _flightCrewReadonlyRepository = flightCrewReadonlyRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<FlightCrewDto>> Handle(GetAllFlightCrewQuery query, CancellationToken token)
    {
        var flightCrewList = await _flightCrewReadonlyRepository.GetAllAsync(token);

        var flightCrewDtoList = _mapper.Map<IReadOnlyList<FlightCrewDto>>(flightCrewList);

        return flightCrewDtoList;
    }
}