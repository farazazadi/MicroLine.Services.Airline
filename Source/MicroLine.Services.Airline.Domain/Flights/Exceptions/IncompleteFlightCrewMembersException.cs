using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.FlightCrews;

namespace MicroLine.Services.Airline.Domain.Flights.Exceptions;

public class IncompleteFlightCrewMembersException : DomainException
{
    public override string Code => nameof(IncompleteFlightCrewMembersException);

    public IncompleteFlightCrewMembersException(FlightCrewType flightCrewType)
        : base($"The presence of at least one {flightCrewType} in the flight crew is mandatory!")
    {
    }

    public IncompleteFlightCrewMembersException(string message) : base(message)
    {
    }
}