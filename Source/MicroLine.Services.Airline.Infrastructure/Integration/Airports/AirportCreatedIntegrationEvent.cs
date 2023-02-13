using MicroLine.Services.Airline.Application.Common.DataTransferObjects;

namespace MicroLine.Services.Airline.Infrastructure.Integration.Airports;
internal class AirportCreatedIntegrationEvent : IntegrationEvent
{
    public override string EventName => nameof(AirportCreatedIntegrationEvent);

    public required string Id { get; init; }
    public required string IcaoCode { get; init; }
    public required string IataCode { get; init; }
    public required string Name { get; init; }
    public required BaseUtcOffsetDto BaseUtcOffset { get; init; }
    public required AirportLocationModel Location { get; init; }


    public record AirportLocationModel(string Country, string Region, string City);

}