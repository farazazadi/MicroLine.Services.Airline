using Mapster;
using MicroLine.Services.Airline.Domain.Airports.Events;
using MicroLine.Services.Airline.Infrastructure.Extensions;
using MicroLine.Services.Airline.Infrastructure.Persistence.Entities;

namespace MicroLine.Services.Airline.Infrastructure.Integration.Airports;

internal class AirportIntegrationEventsMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AirportCreatedEvent, AirportCreatedIntegrationEvent>()
            .Map(iEvent => iEvent.Id, dEvent => dEvent.Airport.Id)
            .Map(iEvent => iEvent.IcaoCode, dEvent => dEvent.Airport.IcaoCode)
            .Map(iEvent => iEvent.IataCode, dEvent => dEvent.Airport.IataCode)
            .Map(iEvent => iEvent.Name, dEvent => dEvent.Airport.Name)
            .Map(iEvent => iEvent.BaseUtcOffset, dEvent => dEvent.Airport.BaseUtcOffset)
            .Map(iEvent => iEvent.Location, dEvent => dEvent.Airport.AirportLocation)
            ;

        config.NewConfig<AirportCreatedIntegrationEvent, OutboxMessage>()
            .MapWith(integrationEvent => OutboxMessage.Create(
                integrationEvent.EventId,
                integrationEvent.EventName,
                integrationEvent.ToJsonString()))
            ;
    }
}
