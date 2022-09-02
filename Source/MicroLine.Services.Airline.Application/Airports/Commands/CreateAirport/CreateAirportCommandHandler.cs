using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Application.Airports.Commands.CreateAirport;

internal class CreateAirportCommandHandler : IRequestHandler<CreateAirportCommand, AirportDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IAirportReadonlyRepository _airportReadonlyRepository;
    private readonly IMapper _mapper;

    public CreateAirportCommandHandler(
        IAirlineDbContext airlineDbContext,
        IAirportReadonlyRepository repository,
        IMapper mapper)
    {
        _airlineDbContext = airlineDbContext;
        _airportReadonlyRepository = repository;
        _mapper = mapper;
    }

    public async Task<AirportDto> Handle(CreateAirportCommand command, CancellationToken token)
    {
        var baseUtcOffset = BaseUtcOffset.Create(
            command.BaseUtcOffsetDto.Hours,
            command.BaseUtcOffsetDto.Minutes);

        var airportLocation = AirportLocation.Create(
            command.AirportLocationDto.Continent,
            command.AirportLocationDto.Country,
            command.AirportLocationDto.Region,
            command.AirportLocationDto.City,
            command.AirportLocationDto.Latitude,
            command.AirportLocationDto.Longitude
            );

        var airport = await Airport.CreateAsync(
            command.IcaoCode,
            command.IataCode,
            command.Name,
            baseUtcOffset,
            airportLocation,

            _airportReadonlyRepository,
            token
        );

        await _airlineDbContext.Airports.AddAsync(airport, token);

        await _airlineDbContext.SaveChangesAsync(token);

        var airportDto = _mapper.Map<AirportDto>(airport);

        return airportDto;
    }
}