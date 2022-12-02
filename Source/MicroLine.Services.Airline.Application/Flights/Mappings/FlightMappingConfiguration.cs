using Mapster;
using MicroLine.Services.Airline.Application.Flights.Commands.ScheduleFlight;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Flights;

namespace MicroLine.Services.Airline.Application.Flights.Mappings;

internal class FlightMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Flight, ScheduleFlightCommand>()
            .Map(command => command.FlightNumber, flight => flight.FlightNumber)
            .Map(command => command.OriginAirportId, flight => flight.OriginAirport.Id)
            .Map(command => command.DestinationAirportId, flight => flight.DestinationAirport.Id)
            .Map(command => command.AircraftId, flight => flight.Aircraft.Id)
            .Map(command => command.ScheduledUtcDateTimeOfDeparture, flight => flight.ScheduledUtcDateTimeOfDeparture)
            .Map(command => command.BasePrices, flight => flight.Prices)
            .Map(command => command.FlightCrewMembers, flight => flight.FlightCrewMembers.Select(fc=> fc.Id))
            .Map(command => command.CabinCrewMembers, flight => flight.CabinCrewMembers.Select(cc=> cc.Id))
            ;

        config.NewConfig<Flight, FlightDto>()
            .Map(dto => dto.Id, flight => flight.Id)
            .Map(dto => dto.OriginAirport, flight => flight.OriginAirport)
            .Map(dto => dto.DestinationAirport, flight => flight.DestinationAirport)
            .Map(dto => dto.Aircraft, flight => flight.Aircraft)
            .Map(dto => dto.ScheduledUtcDateTimeOfDeparture, flight => flight.ScheduledUtcDateTimeOfDeparture)
            .Map(dto => dto.ScheduledUtcDateTimeOfArrival, flight => flight.ScheduledUtcDateTimeOfArrival)
            .Map(dto => dto.EstimatedFlightDuration, flight => flight.EstimatedFlightDuration)
            .Map(dto => dto.Prices, flight => flight.Prices)
            .Map(dto => dto.FlightCrewMembers, flight => flight.FlightCrewMembers)
            .Map(dto => dto.CabinCrewMembers, flight => flight.CabinCrewMembers)
            .Map(dto => dto.Status, flight => flight.Status)
            .Map(dto => dto.AuditingDetails, flight => flight)
            ;
    }
}
