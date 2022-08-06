using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Flights.Exceptions;

public class InvalidScheduledDateTimeOfDeparture : DomainException
{
    public override string Code => nameof(InvalidScheduledDateTimeOfDeparture);

    public InvalidScheduledDateTimeOfDeparture(string message) : base(message)
    {
    }
}