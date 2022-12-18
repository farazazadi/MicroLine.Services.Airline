namespace MicroLine.Services.Airline.Infrastructure.Integration.Flights;

internal class FlightScheduledIntegrationEvent : IntegrationEvent
{
    public override string EventName => nameof(FlightScheduledIntegrationEvent);
    public FlightIntegrationDto Flight { get; }

    public FlightScheduledIntegrationEvent(FlightIntegrationDto flight)
    {
        Flight = flight;
    }
}