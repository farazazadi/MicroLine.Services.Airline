using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Airports;

namespace MicroLine.Services.Airline.Application.Airports.Queries.GetAllAirports;

public class GetAllAirportsQueryHandler : IRequestHandler<GetAllAirportsQuery, IReadOnlyList<AirportDto>>
{
    private readonly IAirportReadonlyRepository _airportReadonlyRepository;
    private readonly IMapper _mapper;

    public GetAllAirportsQueryHandler(
        IAirportReadonlyRepository repository,
        IMapper mapper
        )
    {
        _airportReadonlyRepository = repository;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<AirportDto>> Handle(GetAllAirportsQuery query, CancellationToken token)
    {
        var airports = await _airportReadonlyRepository.GetAllAsync(token);

        var airportDtoList = _mapper.Map<List<AirportDto>>(airports);

        return airportDtoList;
    }
}