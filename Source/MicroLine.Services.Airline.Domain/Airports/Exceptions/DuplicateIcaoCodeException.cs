using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Airports.Exceptions;
public class DuplicateIcaoCodeException : DomainException
{
    public override string Code => nameof(DuplicateIcaoCodeException);

    public DuplicateIcaoCodeException(string icaoCode) : base($"An airport with same IcaoCode ({icaoCode}) already exist!")
    {
    }
}
