using MicroLine.Services.Airline.Application.Common.Exceptions;

namespace MicroLine.Services.Airline.Application.CabinCrews.Commands.CreateCabinCrew;

public class CreateCabinCrewException : ApplicationExceptionBase
{
    public override string Code => nameof(CreateCabinCrewException);

    public CreateCabinCrewException(string message) : base(message)
    {
    }
}
