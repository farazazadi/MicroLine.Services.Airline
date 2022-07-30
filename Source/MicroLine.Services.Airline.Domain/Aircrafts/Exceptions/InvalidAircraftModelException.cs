using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Aircrafts.Exceptions;

public class InvalidAircraftModelException : DomainException
{
    public override string Code => nameof(InvalidAircraftModelException);

    public InvalidAircraftModelException(string message) : base(message)
    {
    }

}