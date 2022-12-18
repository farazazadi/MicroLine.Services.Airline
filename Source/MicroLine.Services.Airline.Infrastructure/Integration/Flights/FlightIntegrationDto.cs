using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;
using MicroLine.Services.Airline.Infrastructure.Integration.Aircrafts;
using MicroLine.Services.Airline.Infrastructure.Integration.Aircrews;
using MicroLine.Services.Airline.Infrastructure.Integration.Airports;

namespace MicroLine.Services.Airline.Infrastructure.Integration.Flights;

internal record FlightIntegrationDto(
    Guid Id,
    string FlightNumber,
    AirportIntegrationDto OriginAirport,
    AirportIntegrationDto DestinationAirport,
    AircraftIntegrationDto Aircraft,
    DateTime ScheduledUtcDateTimeOfDeparture,
    DateTime ScheduledUtcDateTimeOfArrival,
    TimeSpan EstimatedFlightDuration,
    FlightPriceDto Prices,
    List<AircrewIntegrationDto> FlightCrewMembers,
    List<AircrewIntegrationDto> CabinCrewMembers,
    string Status
);