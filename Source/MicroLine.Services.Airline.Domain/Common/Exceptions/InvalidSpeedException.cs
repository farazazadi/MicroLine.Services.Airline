namespace MicroLine.Services.Airline.Domain.Common.Exceptions;

public class InvalidSpeedException : DomainException
{
    public override string Code => nameof(InvalidSpeedException);

    public InvalidSpeedException(int speed) : base($"Speed ({speed}) can not be negative!")
    {
    }

    public InvalidSpeedException(string speed) : base($"Speed can not be created with this string: '{speed}'")
    {
    }
}