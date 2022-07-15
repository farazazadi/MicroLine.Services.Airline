namespace MicroLine.Services.Airline.Domain.Common.Exceptions;

public class InvalidAddressException : DomainException
{
    public override string Code => nameof(InvalidAddressException);

    public InvalidAddressException(string message) : base(message)
    {
    }

}