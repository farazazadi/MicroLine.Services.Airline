using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Airports.Exceptions;

public class InvalidAirportLocationException : DomainException
{
    public override string Code => nameof(InvalidAirportLocationException);

    public InvalidAirportLocationException(string message) : base(message)
    {
    }

}
