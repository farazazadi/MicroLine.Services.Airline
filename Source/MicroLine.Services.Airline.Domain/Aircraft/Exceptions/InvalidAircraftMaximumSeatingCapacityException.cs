using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Aircraft.Exceptions;

public class InvalidAircraftMaximumSeatingCapacityException : DomainException
{
    public override string Code => nameof(InvalidAircraftMaximumSeatingCapacityException);

    protected InvalidAircraftMaximumSeatingCapacityException(string message) : base(message)
    {
    }

    public InvalidAircraftMaximumSeatingCapacityException(int capacity) : base($"Aircraft maximum seating capacity should be greater than {capacity}!")
    {
    }


}
