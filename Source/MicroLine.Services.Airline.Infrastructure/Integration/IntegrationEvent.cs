namespace MicroLine.Services.Airline.Infrastructure.Integration;

public abstract class IntegrationEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public abstract string EventName { get; }
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}