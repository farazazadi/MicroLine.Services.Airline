using MediatR;
using MicroLine.Services.Airline.Domain.Common;

namespace MicroLine.Services.Airline.Application.Common.Models;

public class Notification<TDomainEvent> : INotification where TDomainEvent : DomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public Notification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

}