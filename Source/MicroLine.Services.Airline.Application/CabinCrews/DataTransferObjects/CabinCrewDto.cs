using MicroLine.Services.Airline.Application.Common.DataTransferObjects;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common.Enums;

namespace MicroLine.Services.Airline.Application.CabinCrews.DataTransferObjects;

public record CabinCrewDto(
    string Id,
    CabinCrewType CabinCrewType,
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
