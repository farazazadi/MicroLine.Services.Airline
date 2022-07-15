namespace MicroLine.Services.Airline.Domain.Common.Exceptions;

public class InvalidContactNumberException : DomainException
{
    public override string Code => nameof(InvalidContactNumberException);

    public InvalidContactNumberException(string message) : base(message)
    {
    }

}