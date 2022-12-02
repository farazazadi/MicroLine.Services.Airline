using MediatR;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.Flights.Commands.ScheduleFlight;

public record ScheduleFlightCommand(
    string FlightNumber,
    Guid OriginAirportId,
    Guid DestinationAirportId,
    Guid AircraftId,
    DateTime ScheduledUtcDateTimeOfDeparture,
    FlightPriceDto BasePrices,
    List<Guid> FlightCrewMembers,
    List<Guid> CabinCrewMembers

) : IRequest<FlightDto>;