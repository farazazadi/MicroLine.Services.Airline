namespace MicroLine.Services.Airline.Domain.Common.Exceptions;

public abstract class DomainException : Exception
{
    public abstract string Code { get; }
    protected DomainException(string message) : base(message) {}
}