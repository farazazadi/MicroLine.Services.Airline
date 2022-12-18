using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.Extensions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Domain.FlightPricingPolicies;
using MicroLine.Services.Airline.Domain.Flights.Events;
using MicroLine.Services.Airline.Domain.Flights.Exceptions;


namespace MicroLine.Services.Airline.Domain.Flights;

public class Flight : AggregateRoot
{
    private readonly List<FlightCrew> _flightCrewMembers;
    private readonly List<CabinCrew> _cabinCrewMembers;
    public FlightNumber FlightNumber { get; private set; }
    public Airport OriginAirport { get; private set; }
    public Airport DestinationAirport { get; private set; }
    public Aircraft Aircraft { get; private set; }
    public DateTime ScheduledUtcDateTimeOfDeparture { get; private set; }
    public DateTime ScheduledUtcDateTimeOfArrival { get; private set; }
    public TimeSpan EstimatedFlightDuration { get; private set; }
    public FlightPrice Prices { get; private set; }

    public IReadOnlyList<FlightCrew> FlightCrewMembers => _flightCrewMembers;
    public IReadOnlyList<CabinCrew> CabinCrewMembers => _cabinCrewMembers;

    public FlightStatus Status { get; private set; }

    private Flight() { }

    private Flight(FlightNumber flightNumber,
        Airport originAirport, Airport destinationAirport, Aircraft aircraft,
        DateTime scheduledUtcDateTimeOfDeparture, FlightPrice prices,
        List<FlightCrew> flightCrewMembers, List<CabinCrew> cabinCrewMembers)
    {
        FlightNumber = flightNumber;
        OriginAirport = originAirport;
        DestinationAirport = destinationAirport;
        Aircraft = aircraft;
        ScheduledUtcDateTimeOfDeparture = scheduledUtcDateTimeOfDeparture.RemoveSeconds();
        Prices = prices;
        _flightCrewMembers = flightCrewMembers;
        _cabinCrewMembers = cabinCrewMembers;
    }


    private void CheckScheduledUtcDateTimeOfDeparture()
    {
        var now = DateTime.UtcNow.RemoveSeconds();

        if (ScheduledUtcDateTimeOfDeparture <= now)
            throw new InvalidScheduledDateTimeOfDeparture($"The scheduled DateTime (UTC) of departure ({ScheduledUtcDateTimeOfDeparture:f}) cannot be in the past!");
    }

    private void CheckFlightCrewMembersAsync()
    {
        var pilotExist = FlightCrewMembers
            .Any(f => f.FlightCrewType is FlightCrewType.Pilot);

        if (!pilotExist)
            throw new IncompleteFlightCrewMembersException(FlightCrewType.Pilot);

        var coPilotExist = FlightCrewMembers
            .Any(f => f.FlightCrewType is FlightCrewType.CoPilot);

        if (!coPilotExist)
        {
            var atLeastTwoPilotExist = FlightCrewMembers
                .Count(f => f.FlightCrewType is FlightCrewType.Pilot) > 1;

            if (!atLeastTwoPilotExist)
                throw new IncompleteFlightCrewMembersException(
                    "The presence of at least one Pilot and one Co-Pilot or two Pilots in the flight crew is mandatory!");
        }
        
    }

    private void SetEstimatedFlightDuration()
    {
        var speedKmh = Aircraft.CruisingSpeed.ConvertTo(Speed.UnitOfSpeed.KilometresPerHour);
        var distanceKm = OriginAirport.GetDistanceTo(DestinationAirport, LengthUnit.Kilometer);

        var timeDistanceInHours = distanceKm / speedKmh;

        EstimatedFlightDuration = TimeSpan.FromHours(timeDistanceInHours);
    }

    private void SetScheduledUtcDateTimeOfArrival()
    {
        ScheduledUtcDateTimeOfArrival = ScheduledUtcDateTimeOfDeparture
            .Add(EstimatedFlightDuration)
            .RemoveSeconds();
    }

    private void ApplyPricingPolicies(IEnumerable<IFlightPricingPolicy> flightPricingPolicies)
    {
        var pricingPolicies = flightPricingPolicies
            .OrderBy(p => p.Priority)
            .ToList();

        foreach (var policy in pricingPolicies)
            Prices = policy.Calculate(this);
    }

    public static Flight ScheduleNewFlight(
        IEnumerable<IFlightPricingPolicy> flightPricingPolicies,
        FlightNumber flightNumber, Airport originAirport, Airport destinationAirport, Aircraft aircraft,
        DateTime scheduledUtcDateTimeOfDeparture, FlightPrice basePrices,
        List<FlightCrew> flightCrewMembers, List<CabinCrew> cabinCrewMembers
        )
    {
        
        var flight = new Flight(flightNumber, originAirport, destinationAirport, aircraft,
            scheduledUtcDateTimeOfDeparture, basePrices,
            flightCrewMembers, cabinCrewMembers);

        flight.CheckScheduledUtcDateTimeOfDeparture();

        flight.SetEstimatedFlightDuration();
        flight.SetScheduledUtcDateTimeOfArrival();

        flight.CheckFlightCrewMembersAsync();

        flight.ApplyPricingPolicies(flightPricingPolicies);

        flight.AddEvent(new FlightScheduledEvent(flight));
        flight.Status = FlightStatus.Scheduled;

        return flight;
    }
}