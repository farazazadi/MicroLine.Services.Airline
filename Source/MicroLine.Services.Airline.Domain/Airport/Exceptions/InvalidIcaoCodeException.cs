using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Airport.Exceptions;
public class InvalidIcaoCodeException : DomainException
{
    public override string Code => nameof(InvalidIcaoCodeException);

    public InvalidIcaoCodeException(string message) : base(message)
    {
    }

}