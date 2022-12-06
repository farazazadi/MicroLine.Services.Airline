using MicroLine.Services.Airline.Application.Common.Exceptions;

namespace MicroLine.Services.Airline.Application.FlightCrews.Commands.CreateFlightCrew;

public class CreateFlightCrewException : ApplicationExceptionBase
{
    public override string Code => nameof(CreateFlightCrewException);

    public CreateFlightCrewException(string message) : base(message)
    {
    }
}
