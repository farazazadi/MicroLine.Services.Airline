namespace MicroLine.Services.Airline.Domain.Common.Exceptions;

public class InvalidPassportNumberException : DomainException
{
    public override string Code => nameof(InvalidPassportNumberException);

    public InvalidPassportNumberException(string message) : base(message)
    {
    }

}