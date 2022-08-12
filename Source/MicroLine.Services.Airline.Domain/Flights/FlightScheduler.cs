using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Domain.FlightPricingPolicies;

namespace MicroLine.Services.Airline.Domain.Flights;

public partial class Flight
{
    public static class Scheduler
    {
        public static Flight ScheduleNewFlight(IFlightRepository flightRepository, IEnumerable<IFlightPricingPolicy> flightPricingPolicies,
                                    FlightNumber flightNumber, Airport originAirport, Airport destinationAirport, Aircraft aircraft,
                                    DateTime scheduledUtcDateTimeOfDeparture, FlightPrice basePrices,
                                    List<FlightCrew> flightCrewMembers, List<CabinCrew> cabinCrewMembers)
        {
            var flight = new Flight(flightNumber, originAirport, destinationAirport, aircraft,
                                    scheduledUtcDateTimeOfDeparture, basePrices,
                                    flightCrewMembers, cabinCrewMembers);



            var flightCrewMembersLastFlight = flightRepository.GetLastFlightOf(flightCrewMembers);
            var cabinCrewMembersLastFlight = flightRepository.GetLastFlightOf(cabinCrewMembers);
            var aircraftLastFlight = flightRepository.GetLastFlightOf(aircraft);

            flight.CheckInvariants(aircraftLastFlight, flightCrewMembersLastFlight, cabinCrewMembersLastFlight);

            flight.ApplyPricingPolicies(flightPricingPolicies);

            flight.Schedule();

            return flight;
        }
    }
}