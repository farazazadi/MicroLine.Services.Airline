using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Flights.Exceptions;

public class InvalidFlightPriceException : DomainException
{
    public override string Code => nameof(InvalidFlightPriceException);

    public InvalidFlightPriceException(string message) : base(message)
    {
    }

}