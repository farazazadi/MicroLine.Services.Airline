namespace MicroLine.Services.Airline.Domain.Common.Exceptions;

public class DuplicateNationalIdException : DomainException
{
    public override string Code => nameof(DuplicateNationalIdException);

    public DuplicateNationalIdException(string message) : base(message)
    {
    }
}
