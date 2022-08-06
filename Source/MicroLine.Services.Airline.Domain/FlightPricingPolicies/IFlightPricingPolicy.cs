using MicroLine.Services.Airline.Domain.Flights;

namespace MicroLine.Services.Airline.Domain.FlightPricingPolicies;
public interface IFlightPricingPolicy
{
    byte Priority { get; }
    FlightPrice Calculate(Flight flight);
}
