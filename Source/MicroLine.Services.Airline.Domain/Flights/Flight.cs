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

public partial class Flight : AggregateRoot
{
    public FlightNumber FlightNumber { get; private set; }
    public Airport OriginAirport { get; private set; }
    public Airport DestinationAirport { get; private set; }
    public Aircraft Aircraft { get; private set; }
    public DateTime ScheduledUtcDateTimeOfDeparture { get; private set; }
    public DateTime ScheduledUtcDateTimeOfArrival { get; private set; }
    public TimeSpan EstimatedFlightDuration { get; private set; }
    public FlightPrice Prices { get; private set; }
    public List<FlightCrew> FlightCrewMembers { get; private set; }
    public List<CabinCrew> CabinCrewMembers { get; private set; }
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
        FlightCrewMembers = flightCrewMembers;
        CabinCrewMembers = cabinCrewMembers;
    }


    private void CheckInvariants(Flight aircraftLastFlight,
                            IReadOnlyList<Flight> flightCrewMembersLastFlight,
                            IReadOnlyList<Flight> cabinCrewMembersLastFlight)
    {
        CheckScheduledUtcDateTimeOfDeparture();

        CheckFlightCrewMembers(flightCrewMembersLastFlight);
        CheckCabinCrewMembersAvailability(cabinCrewMembersLastFlight);
        CheckAircraftAvailability(aircraftLastFlight);
    }


    private void CheckScheduledUtcDateTimeOfDeparture()
    {
        var now = DateTime.UtcNow.RemoveSeconds();

        if (ScheduledUtcDateTimeOfDeparture <= now)
            throw new InvalidScheduledDateTimeOfDeparture($"The scheduled DateTime (UTC) of departure ({ScheduledUtcDateTimeOfDeparture:f}) cannot be in the past!");
    }

    private void CheckFlightCrewMembers(IReadOnlyList<Flight> flightCrewMembersLastFlight)
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


        //todo: Availability of all FlightCrewMembers should be checked
    }


    private void CheckCabinCrewMembersAvailability(IReadOnlyList<Flight> cabinCrewMembersLastFlight)
    {
        //if (cabinCrewMembersLastFlight is null)
        //    return;

        //todo: Availability of all cabinCrewMembers should be checked
    }

    private void CheckAircraftAvailability(Flight aircraftLastFlight)
    {
        //todo: Availability of Aircraft should be checked
    }


    private void ApplyPricingPolicies(IEnumerable<IFlightPricingPolicy> flightPricingPolicies)
    {
        var pricingPolicies = flightPricingPolicies
            .OrderBy(p => p.Priority)
            .ToList();

        foreach (var policy in pricingPolicies)
            Prices = policy.Calculate(this);
    }

    private void Schedule()
    {
        SetEstimatedFlightDuration();
        SetScheduledUtcDateTimeOfArrival();
        AddEvent(new FlightScheduledEvent(Id));
        Status = FlightStatus.Scheduled;
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

}