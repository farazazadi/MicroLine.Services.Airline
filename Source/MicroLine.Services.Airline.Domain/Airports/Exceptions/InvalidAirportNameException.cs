using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Airports.Exceptions;

public class InvalidAirportNameException : DomainException
{
    public override string Code => nameof(InvalidAirportNameException);

    public InvalidAirportNameException(string message) : base(message)
    {
    }

}

