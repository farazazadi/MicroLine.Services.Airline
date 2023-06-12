using Mapster;
using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.Flights.Events;
using MicroLine.Services.Airline.Infrastructure.Extensions;
using MicroLine.Services.Airline.Infrastructure.Persistence.Entities;

namespace MicroLine.Services.Airline.Infrastructure.Integration.Flights;
internal class FlightIntegrationEventsMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Airport, FlightScheduledIntegrationEvent.AirportModel>()
            .MapToConstructor(false)
            .Map(model => model.Country, airport => airport.AirportLocation.Country)
            .Map(model => model.Region, airport => airport.AirportLocation.Region)
            .Map(model => model.City, airport => airport.AirportLocation.City)
            ;

        config.NewConfig<Aircraft, FlightScheduledIntegrationEvent.AircraftModel>()
            .MapToConstructor(false)
            .Map(model => model.Model, aircraft => $"{aircraft.Manufacturer} {aircraft.Model}")
            .Map(model => model.EconomyClassCapacity, aircraft => aircraft.PassengerSeatingCapacity.EconomyClassCapacity)
            .Map(model => model.BusinessClassCapacity, aircraft => aircraft.PassengerSeatingCapacity.BusinessClassCapacity)
            .Map(model => model.FirstClassCapacity, aircraft => aircraft.PassengerSeatingCapacity.FirstClassCapacity)
            ;


        config.NewConfig<FlightScheduledEvent, FlightScheduledIntegrationEvent>()
            .Map(iEvent => iEvent.FlightId, dEvent => dEvent.Flight.Id)
            .Map(iEvent => iEvent.FlightNumber, dEvent => dEvent.Flight.FlightNumber)
            .Map(iEvent => iEvent.OriginAirport, dEvent => dEvent.Flight.OriginAirport)
            .Map(iEvent => iEvent.DestinationAirport, dEvent => dEvent.Flight.DestinationAirport)
            .Map(iEvent => iEvent.Aircraft, dEvent => dEvent.Flight.Aircraft)
            .Map(iEvent => iEvent.ScheduledUtcDateTimeOfDeparture, dEvent => dEvent.Flight.ScheduledUtcDateTimeOfDeparture)
            .Map(iEvent => iEvent.ScheduledUtcDateTimeOfArrival, dEvent => dEvent.Flight.ScheduledUtcDateTimeOfArrival)
            .Map(iEvent => iEvent.EstimatedFlightDuration, dEvent => dEvent.Flight.EstimatedFlightDuration)
            .Map(iEvent => iEvent.Prices, dEvent => dEvent.Flight.Prices)
            .Map(iEvent => iEvent.Status, dEvent => dEvent.Flight.Status)
            ;

        config.NewConfig<FlightScheduledIntegrationEvent, OutboxMessage>()
            .MapWith(integrationEvent => OutboxMessage.Create(
                integrationEvent.EventId,
                integrationEvent.EventName,
                integrationEvent.ToJsonString()))
            ;

    }
}