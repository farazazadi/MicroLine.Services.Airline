namespace MicroLine.Services.Airline.Domain.Common.Exceptions;
public class InvalidNationalIdException : DomainException
{
    public override string Code => nameof(InvalidNationalIdException);

    public InvalidNationalIdException(string message) : base(message)
    {
    }

}