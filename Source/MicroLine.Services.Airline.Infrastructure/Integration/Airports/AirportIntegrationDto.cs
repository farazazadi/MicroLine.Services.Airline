using MicroLine.Services.Airline.Application.Common.DataTransferObjects;

namespace MicroLine.Services.Airline.Infrastructure.Integration.Airports;

internal record AirportIntegrationDto(
    string Id,
    string IcaoCode,
    string IataCode,
    string Name,
    BaseUtcOffsetDto BaseUtcOffset,
    string Country,
    string Region,
    string City
);