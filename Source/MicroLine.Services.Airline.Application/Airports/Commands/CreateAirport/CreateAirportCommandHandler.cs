using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.Airports.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Application.Airports.Commands.CreateAirport;

internal class CreateAirportCommandHandler : IRequestHandler<CreateAirportCommand, AirportDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public CreateAirportCommandHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper)
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }

    public async Task<AirportDto> Handle(CreateAirportCommand command, CancellationToken token)
    {
        IcaoCode icaoCode = command.IcaoCode;

        var icaoCodeExist = await _airlineDbContext.Airports
            .AnyAsync(airport => airport.IcaoCode == icaoCode, token);

        if (icaoCodeExist)
            throw new DuplicateIcaoCodeException(icaoCode);


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

        var airport = Airport.Create(
            icaoCode,
            command.IataCode,
            command.Name,
            baseUtcOffset,
            airportLocation
        );

        await _airlineDbContext.Airports.AddAsync(airport, token);

        await _airlineDbContext.SaveChangesAsync(token);

        var airportDto = _mapper.Map<AirportDto>(airport);

        return airportDto;
    }
}