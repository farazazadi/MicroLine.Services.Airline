using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Airports;

namespace MicroLine.Services.Airline.Application.Airports.Queries.GetAirportById;

public class GetAirportByIdQueryHandler : IRequestHandler<GetAirportByIdQuery, AirportDto>
{
    private readonly IAirportReadonlyRepository _airportReadonlyRepository;
    private readonly IMapper _mapper;

    public GetAirportByIdQueryHandler(
        IAirportReadonlyRepository repository,
        IMapper mapper
        )
    {
        _airportReadonlyRepository = repository;
        _mapper = mapper;
    }

    public async Task<AirportDto> Handle(GetAirportByIdQuery query, CancellationToken token)
    {
        var airport = await _airportReadonlyRepository.GetAsync(query.Id, token);

        return airport is null ? null : _mapper.Map<AirportDto>(airport);
    }
}