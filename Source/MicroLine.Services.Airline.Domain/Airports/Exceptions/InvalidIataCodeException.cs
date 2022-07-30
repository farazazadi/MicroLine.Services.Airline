using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Airports.Exceptions;

public class InvalidIataCodeException : DomainException
{
    public override string Code => nameof(InvalidIataCodeException);

    public InvalidIataCodeException(string message) : base(message)
    {
    }

}