using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Flights.Exceptions;

public class OverlapFlightResourcesException : DomainException
{
    public override string Code => nameof(OverlapFlightResourcesException);

    public OverlapFlightResourcesException(string message) : base(message)
    {
    }
}
