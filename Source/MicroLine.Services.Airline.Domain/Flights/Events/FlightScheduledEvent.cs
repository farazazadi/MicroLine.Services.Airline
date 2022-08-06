using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.Flights.Events;
public class FlightScheduledEvent : DomainEvent
{
    public Id FlightId { get;}

    public FlightScheduledEvent(Id flightId)
    {
        FlightId = flightId;
    }
}
