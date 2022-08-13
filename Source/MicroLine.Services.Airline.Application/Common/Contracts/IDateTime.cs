namespace MicroLine.Services.Airline.Application.Common.Contracts;

public interface IDateTime
{
    DateTimeOffset Now { get; }
}