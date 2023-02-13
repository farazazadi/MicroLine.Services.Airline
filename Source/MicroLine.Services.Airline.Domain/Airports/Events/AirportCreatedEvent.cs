using MicroLine.Services.Airline.Domain.Common;

namespace MicroLine.Services.Airline.Domain.Airports.Events;

public class AirportCreatedEvent : DomainEvent
{
    public Airport Airport { get; }

    public AirportCreatedEvent(Airport airport)
    {
        Airport = airport;
    }
}
