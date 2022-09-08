using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Aircrafts;

namespace MicroLine.Services.Airline.Application.Aircrafts.Queries.GetAllAircrafts;

public class GetAllAircraftsQueryHandler : IRequestHandler<GetAllAircraftsQuery, IReadOnlyList<AircraftDto>>
{
    private readonly IAircraftReadonlyRepository _aircraftReadonlyRepository;
    private readonly IMapper _mapper;

    public GetAllAircraftsQueryHandler(
        IAircraftReadonlyRepository aircraftReadonlyRepository,
        IMapper mapper)
    {
        _aircraftReadonlyRepository = aircraftReadonlyRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AircraftDto>> Handle(GetAllAircraftsQuery query, CancellationToken token)
    {
        var aircrafts = await _aircraftReadonlyRepository.GetAllAsync(token);

        var aircraftDtoList = _mapper.Map<IReadOnlyList<AircraftDto>>(aircrafts);

        return aircraftDtoList;
    }
}