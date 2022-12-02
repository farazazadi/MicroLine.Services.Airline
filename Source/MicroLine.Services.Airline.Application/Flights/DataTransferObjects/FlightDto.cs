using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;
using MicroLine.Services.Airline.Application.CabinCrews.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.DataTransferObjects;
using MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Flights;

namespace MicroLine.Services.Airline.Application.Flights.DataTransferObjects;

public record FlightDto(
    Guid Id,
    string FlightNumber,
    AirportDto OriginAirport,
    AirportDto DestinationAirport,
    AircraftDto Aircraft,
    DateTime ScheduledUtcDateTimeOfDeparture,
    DateTime ScheduledUtcDateTimeOfArrival,
    TimeSpan EstimatedFlightDuration,
    FlightPriceDto Prices,
    List<FlightCrewDto> FlightCrewMembers,
    List<CabinCrewDto> CabinCrewMembers,
    FlightStatus Status,
    EntityAuditingDetailsDto AuditingDetails
);