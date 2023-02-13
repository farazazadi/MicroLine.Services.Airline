using MapsterMapper;
using MicroLine.Services.Airline.Domain.Airports.Events;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Flights.Events;
using MicroLine.Services.Airline.Infrastructure.Integration.Airports;
using MicroLine.Services.Airline.Infrastructure.Integration.Flights;

namespace MicroLine.Services.Airline.Infrastructure.Integration;
internal static class IntegrationExtensions
{
    public static IntegrationEvent MapToIntegrationEvent(this DomainEvent domainEvent, IMapper mapper)
    {
        return domainEvent switch
        {
            FlightScheduledEvent e => mapper.Map<FlightScheduledIntegrationEvent>(e),

            AirportCreatedEvent e => mapper.Map<AirportCreatedIntegrationEvent>(e),

            _ => null
        };
    }
}
