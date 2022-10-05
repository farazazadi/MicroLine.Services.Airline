using Mapster;
using MicroLine.Services.Airline.Application.FlightCrews.Commands.CreateFlightCrew;
using MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;

namespace MicroLine.Services.Airline.Application.FlightCrews.Mappings;

internal class FlightCrewMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FlightCrew, CreateFlightCrewCommand>()
            .Map(command => command.FlightCrewType, flightCrew => flightCrew.FlightCrewType)
            .Map(command => command.Gender, flightCrew => flightCrew.Gender)
            .Map(command => command.FullName, flightCrew => flightCrew.FullName)
            .Map(command => command.BirthDate, flightCrew => flightCrew.BirthDate)
            .Map(command => command.NationalId, flightCrew => flightCrew.NationalId)
            .Map(command => command.PassportNumber, flightCrew => flightCrew.PassportNumber)
            .Map(command => command.Email, flightCrew => flightCrew.Email)
            .Map(command => command.ContactNumber, flightCrew => flightCrew.ContactNumber)
            .Map(command => command.Address, flightCrew => flightCrew.Address)
            ;

        config.NewConfig<FlightCrew, FlightCrewDto>()
            .Map(dto => dto.Id, flightCrew => flightCrew.Id)
            .Map(dto => dto.FlightCrewType, flightCrew => flightCrew.FlightCrewType)
            .Map(dto => dto.Gender, flightCrew => flightCrew.Gender)
            .Map(dto => dto.FullName, flightCrew => flightCrew.FullName)
            .Map(dto => dto.BirthDate, flightCrew => flightCrew.BirthDate)
            .Map(dto => dto.NationalId, flightCrew => flightCrew.NationalId)
            .Map(dto => dto.PassportNumber, flightCrew => flightCrew.PassportNumber)
            .Map(dto => dto.Email, flightCrew => flightCrew.Email)
            .Map(dto => dto.ContactNumber, flightCrew => flightCrew.ContactNumber)
            .Map(dto => dto.Address, flightCrew => flightCrew.Address)
            .Map(dto => dto.AuditingDetails, flightCrew => flightCrew)
            ;
    }
}
