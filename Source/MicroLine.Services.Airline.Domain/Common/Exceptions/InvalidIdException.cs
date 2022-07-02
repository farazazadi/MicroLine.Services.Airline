using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.Common.Exceptions;

public class InvalidIdException : DomainException
{
    public override string Code => nameof(InvalidIdException);
    public Id Id { get; }

    public InvalidIdException(Id id) : base($"Invalid Id: {id}") => Id = id;
    
}