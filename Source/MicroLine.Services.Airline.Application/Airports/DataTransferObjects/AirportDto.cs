using MicroLine.Services.Airline.Application.Common.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.Airports.DataTransferObjects;

public record AirportDto(
    string Id,
    string IcaoCode,
    string IataCode,
    string Name,
    BaseUtcOffsetDto BaseUtcOffset,
    AirportLocationDto AirportLocation,
    EntityAuditingDetailsDto AuditingDetails
);