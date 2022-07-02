namespace MicroLine.Services.Airline.Domain.Common;

public abstract class AggregateRoot : AuditableEntity
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;

    protected void AddEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearEvents() => _domainEvents.Clear();
    
}