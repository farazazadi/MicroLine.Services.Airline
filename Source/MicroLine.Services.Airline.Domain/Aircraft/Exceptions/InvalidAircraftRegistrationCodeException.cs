using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Aircraft.Exceptions;

public class InvalidAircraftRegistrationCodeException : DomainException
{
    public override string Code => nameof(InvalidAircraftRegistrationCodeException);

    public InvalidAircraftRegistrationCodeException(string message) : base(message)
    {
    }

}