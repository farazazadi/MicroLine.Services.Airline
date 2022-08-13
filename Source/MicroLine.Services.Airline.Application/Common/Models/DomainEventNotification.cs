using MediatR;
using MicroLine.Services.Airline.Domain.Common;

namespace MicroLine.Services.Airline.Application.Common.Models;

public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : DomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

}