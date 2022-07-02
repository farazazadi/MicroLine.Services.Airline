using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.Common;

public abstract class DomainEvent
{
    public Id Id { get; } = Id.Create();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.Now;

}