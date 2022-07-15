namespace MicroLine.Services.Airline.Domain.Common.Exceptions;

public class InvalidFullNameException : DomainException
{
    public override string Code => nameof(InvalidFullNameException);

    public InvalidFullNameException(string message) : base(message)
    {
    }
}