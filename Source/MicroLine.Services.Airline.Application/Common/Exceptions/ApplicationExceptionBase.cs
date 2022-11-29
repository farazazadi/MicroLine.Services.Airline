namespace MicroLine.Services.Airline.Application.Common.Exceptions;

public abstract class ApplicationExceptionBase : Exception
{
    public abstract string Code { get; }
    protected ApplicationExceptionBase(string message) : base(message) { }
}
