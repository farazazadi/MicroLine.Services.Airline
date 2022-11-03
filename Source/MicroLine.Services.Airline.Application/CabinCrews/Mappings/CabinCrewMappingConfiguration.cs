using Mapster;
using MicroLine.Services.Airline.Application.CabinCrews.Commands.CreateCabinCrew;
using MicroLine.Services.Airline.Application.CabinCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.CabinCrews;

namespace MicroLine.Services.Airline.Application.CabinCrews.Mappings;

internal class CabinCrewMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CabinCrew, CreateCabinCrewCommand>()
            .Map(command => command.CabinCrewType, cabinCrew => cabinCrew.CabinCrewType)
            .Map(command => command.Gender, cabinCrew => cabinCrew.Gender)
            .Map(command => command.FullName, flightCrew => flightCrew.FullName)
            .Map(command => command.BirthDate, flightCrew => flightCrew.BirthDate)
            .Map(command => command.NationalId, flightCrew => flightCrew.NationalId)
            .Map(command => command.PassportNumber, flightCrew => flightCrew.PassportNumber)
            .Map(command => command.Email, flightCrew => flightCrew.Email)
            .Map(command => command.ContactNumber, flightCrew => flightCrew.ContactNumber)
            .Map(command => command.Address, flightCrew => flightCrew.Address)
            ;


        config.NewConfig<CabinCrew, CabinCrewDto>()
            .Map(dto => dto.Id, cabinCrew => cabinCrew.Id)
            .Map(dto => dto.CabinCrewType, cabinCrew => cabinCrew.CabinCrewType)
            .Map(dto => dto.Gender, cabinCrew => cabinCrew.Gender)
            .Map(dto => dto.FullName, cabinCrew => cabinCrew.FullName)
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
