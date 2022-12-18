using Mapster;
using MicroLine.Services.Airline.Domain.Airports;

namespace MicroLine.Services.Airline.Infrastructure.Integration.Airports;

internal class AirportIntegrationMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<Airport, AirportIntegrationDto>()
            .Map(dto => dto.Id, airport => airport.Id)
            .Map(dto => dto.IcaoCode, airport => airport.IcaoCode)
            .Map(dto => dto.IataCode, airport => airport.IataCode)
            .Map(dto => dto.Name, airport => airport.Name)
            .Map(dto => dto.BaseUtcOffset, airport => airport.BaseUtcOffset)
            .Map(dto => dto.Country, airport => airport.AirportLocation.Country)
            .Map(dto => dto.Region, airport => airport.AirportLocation.Region)
            .Map(dto => dto.City, airport => airport.AirportLocation.City)
            ;
    }

}