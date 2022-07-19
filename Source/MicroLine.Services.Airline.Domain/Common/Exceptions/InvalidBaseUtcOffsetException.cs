namespace MicroLine.Services.Airline.Domain.Common.Exceptions;
public class InvalidBaseUtcOffsetException : DomainException
{
    public override string Code => nameof(InvalidBaseUtcOffsetException);

    public InvalidBaseUtcOffsetException() : base("BaseUtcOffset is not valid!")
    {
    }
}
