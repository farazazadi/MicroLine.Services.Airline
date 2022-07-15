namespace MicroLine.Services.Airline.Domain.Common.Exceptions;

public class InvalidEmailException : DomainException
{
    public override string Code => nameof(InvalidEmailException);

    public InvalidEmailException(string message) : base(message)
    {
    }

}