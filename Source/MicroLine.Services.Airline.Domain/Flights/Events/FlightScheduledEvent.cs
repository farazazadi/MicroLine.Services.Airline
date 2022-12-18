using MicroLine.Services.Airline.Domain.Common;

namespace MicroLine.Services.Airline.Domain.Flights.Events;
public class FlightScheduledEvent : DomainEvent
{
    public Flight Flight { get;}

    public FlightScheduledEvent(Flight flight)
    {
        Flight = flight;
    }
}
