using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Flights.Exceptions;

public class InvalidFlightNumberException : DomainException
{
    public override string Code => nameof(InvalidFlightNumberException);

    public InvalidFlightNumberException() : base("FlightNumber is not valid!")
    {
    }

    public InvalidFlightNumberException(string message) : base(message)
    {
    }

}