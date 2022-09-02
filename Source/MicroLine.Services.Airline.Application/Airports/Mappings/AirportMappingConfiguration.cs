using Mapster;
using MicroLine.Services.Airline.Application.Airports.Commands.CreateAirport;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Airports;

namespace MicroLine.Services.Airline.Application.Airports.Mappings;

internal class AirportMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Airport, CreateAirportCommand>()
            .Map(command => command.IcaoCode, airport => airport.IcaoCode)
            .Map(command => command.IataCode, airport => airport.IataCode)
            .Map(command => command.Name, airport => airport.Name)
            .Map(command => command.BaseUtcOffsetDto, airport => airport.BaseUtcOffset)
            .Map(command => command.AirportLocationDto, airport => airport.AirportLocation);


        config.NewConfig<Airport, AirportDto>()
            .Map(dto => dto.Id, airport => airport.Id)
            .Map(dto => dto.IcaoCode, airport => airport.IcaoCode)
            .Map(dto => dto.IataCode, airport => airport.IataCode)
            .Map(dto => dto.Name, airport => airport.Name)
            .Map(dto => dto.BaseUtcOffset, airport => airport.BaseUtcOffset)
            .Map(dto => dto.AirportLocation, airport => airport.AirportLocation)
            .Map(dto => dto.AuditingDetails, airport => airport);

    }

}