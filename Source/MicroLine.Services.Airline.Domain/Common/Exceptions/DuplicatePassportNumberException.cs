
namespace MicroLine.Services.Airline.Domain.Common.Exceptions;
public class DuplicatePassportNumberException : DomainException
{
    public override string Code => nameof(DuplicatePassportNumberException);

    public DuplicatePassportNumberException(string message) : base(message)
    {
    }
}
