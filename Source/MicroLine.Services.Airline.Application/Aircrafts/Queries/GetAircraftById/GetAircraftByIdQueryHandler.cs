using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Aircrafts;

namespace MicroLine.Services.Airline.Application.Aircrafts.Queries.GetAircraftById;

internal class GetAircraftByIdQueryHandler : IRequestHandler<GetAircraftByIdQuery, AircraftDto>
{
    private readonly IAircraftReadonlyRepository _aircraftReadonlyRepository;
    private readonly IMapper _mapper;

    public GetAircraftByIdQueryHandler(
        IAircraftReadonlyRepository aircraftReadonlyRepository,
        IMapper mapper
        )
    {
        _aircraftReadonlyRepository = aircraftReadonlyRepository;
        _mapper = mapper;
    }


    public async Task<AircraftDto> Handle(GetAircraftByIdQuery query, CancellationToken token)
    {
        var aircraft = await _aircraftReadonlyRepository.GetAsync(query.Id, token);

        var aircraftDto = _mapper.Map<AircraftDto>(aircraft);

        return aircraftDto;
    }
}