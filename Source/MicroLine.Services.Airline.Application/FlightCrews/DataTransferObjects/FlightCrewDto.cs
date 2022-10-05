using MicroLine.Services.Airline.Application.Common.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.FlightCrews;

namespace MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;
public record FlightCrewDto(
    string Id,
    FlightCrewType FlightCrewType,
    Gender Gender,
    FullNameDto FullName,
    DateTime BirthDate,
    string NationalId,
    string PassportNumber,
    string Email,
    string ContactNumber,
    AddressDto Address,
    EntityAuditingDetailsDto AuditingDetails
);