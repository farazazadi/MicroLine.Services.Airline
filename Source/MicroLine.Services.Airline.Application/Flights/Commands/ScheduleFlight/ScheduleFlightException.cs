using MicroLine.Services.Airline.Application.Common.Exceptions;

namespace MicroLine.Services.Airline.Application.Flights.Commands.ScheduleFlight;
public class ScheduleFlightException : ApplicationExceptionBase
{
    public override string Code => nameof(ScheduleFlightException);

    public ScheduleFlightException(string message) : base(message)
    {
    }
}
