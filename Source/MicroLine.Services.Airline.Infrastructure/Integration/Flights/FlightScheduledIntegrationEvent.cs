using MicroLine.Services.Airline.Application.Common.DataTransferObjects;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;

namespace MicroLine.Services.Airline.Infrastructure.Integration.Flights;

internal class FlightScheduledIntegrationEvent : IntegrationEvent
{
    public override string EventName => nameof(FlightScheduledIntegrationEvent);

    public required Guid FlightId { get; init; }
    public required string FlightNumber { get; init; }
    public required AirportModel OriginAirport { get; init; }
    public required AirportModel DestinationAirport { get; init; }
    public required AircraftModel Aircraft { get; init; }
    public required DateTime ScheduledUtcDateTimeOfDeparture { get; init; }
    public required DateTime ScheduledUtcDateTimeOfArrival { get; init; }
    public required TimeSpan EstimatedFlightDuration { get; init; }
    public required FlightPriceDto Prices { get; init; }
    public required string Status { get; init; }


    public record AirportModel(
        string IataCode,
        string Name,
        BaseUtcOffsetDto BaseUtcOffset,
        string Country,
        string Region,
        string City
    );

    public record AircraftModel(
        string Model,
        int EconomyClassCapacity,
        int BusinessClassCapacity,
        int FirstClassCapacity
    );

}