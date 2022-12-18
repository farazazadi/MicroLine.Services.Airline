using Mapster;
using MicroLine.Services.Airline.Domain.Flights;
using MicroLine.Services.Airline.Domain.Flights.Events;
using MicroLine.Services.Airline.Infrastructure.Extensions;
using MicroLine.Services.Airline.Infrastructure.Persistence.Entities;

namespace MicroLine.Services.Airline.Infrastructure.Integration.Flights;
internal class FlightIntegrationEventsMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Flight, FlightIntegrationDto>()
            .Map(dto => dto.Aircraft, flight => flight.Aircraft)
            .Map(dto => dto.OriginAirport, flight => flight.OriginAirport)
            .Map(dto => dto.DestinationAirport, flight => flight.DestinationAirport)
            .Map(dto => dto.FlightCrewMembers, flight => flight.FlightCrewMembers)
            .Map(dto => dto.CabinCrewMembers, flight => flight.CabinCrewMembers)
            ;

        config.NewConfig<FlightScheduledEvent, FlightScheduledIntegrationEvent>()
            .Map(integrationEvent => integrationEvent.Flight, domainEvent => domainEvent.Flight)
            ;

        config.NewConfig<FlightScheduledIntegrationEvent, OutboxMessage>()
            .MapWith(integrationEvent => OutboxMessage.Create(
                integrationEvent.EventId,
                integrationEvent.EventName,
                integrationEvent.ToJsonString()))
            ;

    }
}
