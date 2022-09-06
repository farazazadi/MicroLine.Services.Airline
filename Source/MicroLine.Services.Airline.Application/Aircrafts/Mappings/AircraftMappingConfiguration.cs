
using Mapster;
using MicroLine.Services.Airline.Application.Aircrafts.Commands.CreateAircraft;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Aircrafts;

namespace MicroLine.Services.Airline.Application.Aircrafts.Mappings;

internal class AircraftMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Aircraft, CreateAircraftCommand>()
            .Map(command => command.Manufacturer, aircraft => aircraft.Manufacturer)
            .Map(command => command.Model, aircraft => aircraft.Model)
            .Map(command => command.ManufactureDate, aircraft => aircraft.ManufactureDate)
            .Map(command => command.PassengerSeatingCapacity, aircraft => aircraft.PassengerSeatingCapacity)
            .Map(command => command.CruisingSpeed, aircraft => aircraft.CruisingSpeed)
            .Map(command => command.MaximumOperatingSpeed, aircraft => aircraft.MaximumOperatingSpeed)
            .Map(command => command.RegistrationCode, aircraft => aircraft.RegistrationCode)
            ;

        config.NewConfig<Aircraft, AircraftDto>()
            .Map(dto => dto.Id, aircraft => aircraft.Id)
            .Map(dto => dto.Manufacturer, aircraft => aircraft.Manufacturer)
            .Map(dto => dto.Model, aircraft => aircraft.Model)
            .Map(dto => dto.ManufactureDate, aircraft => aircraft.ManufactureDate)
            .Map(dto => dto.PassengerSeatingCapacity, aircraft => aircraft.PassengerSeatingCapacity)
            .Map(dto => dto.CruisingSpeed, aircraft => aircraft.CruisingSpeed)
            .Map(dto => dto.MaximumOperatingSpeed, aircraft => aircraft.MaximumOperatingSpeed)
            .Map(dto => dto.RegistrationCode, aircraft => aircraft.RegistrationCode)
            .Map(dto => dto.AuditingDetails, aircraft => aircraft)
            ;
    }
}
