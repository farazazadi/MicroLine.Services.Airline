using Mapster;
using MicroLine.Services.Airline.Domain.Aircrafts;

namespace MicroLine.Services.Airline.Infrastructure.Integration.Aircrafts;

internal class AircraftIntegrationMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Aircraft, AircraftIntegrationDto>()
            .Map(dto => dto.Id, aircraft => aircraft.Id)
            .Map(dto => dto.Manufacturer, aircraft => aircraft.Manufacturer.ToString())
            .Map(dto => dto.Model, aircraft => aircraft.Model)
            .Map(dto => dto.EconomyClassCapacity, aircraft => aircraft.PassengerSeatingCapacity.EconomyClassCapacity)
            .Map(dto => dto.BusinessClassCapacity, aircraft => aircraft.PassengerSeatingCapacity.BusinessClassCapacity)
            .Map(dto => dto.FirstClassCapacity, aircraft => aircraft.PassengerSeatingCapacity.FirstClassCapacity)
            ;
    }
}
