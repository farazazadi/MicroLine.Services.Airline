using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Airport.Exceptions;

public class InvalidAirportLocationException : DomainException
{
    public override string Code => nameof(InvalidAirportLocationException);

    public InvalidAirportLocationException(string message) : base(message)
    {
    }

}
