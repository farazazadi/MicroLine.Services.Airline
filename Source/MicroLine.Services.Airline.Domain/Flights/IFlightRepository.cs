using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.FlightCrews;

namespace MicroLine.Services.Airline.Domain.Flights;
public interface IFlightRepository
{
    Flight GetLastFlightOf(FlightCrew flightCrew);
    IReadOnlyList<Flight> GetLastFlightOf(List<FlightCrew> flightCrewMembers);
    Flight GetLastFlightOf(CabinCrew cabinCrew);
    IReadOnlyList<Flight> GetLastFlightOf(List<CabinCrew> cabinCrew);
    Flight GetLastFlightOf(Aircraft aircraft);
}
