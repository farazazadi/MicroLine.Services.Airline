namespace MicroLine.Services.Airline.Application.Common.Exceptions;
internal class EntityNotFoundException : Exception
{
    public EntityNotFoundException() : base()
    {}

    public EntityNotFoundException(string message) : base(message)
    {}

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
    {}


    public EntityNotFoundException(string name, object id)
        : base($"'{name}' ({id}) was not found.")
    {
    }
}

