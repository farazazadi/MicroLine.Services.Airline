using MediatR;
using MicroLine.Services.Airline.Application.Common.Models;
using MicroLine.Services.Airline.Domain.Common;
using Microsoft.Extensions.Logging;
using MicroLine.Services.Airline.Application.Common.Contracts;

namespace MicroLine.Services.Airline.Infrastructure.Services;

internal class EventDispatcherService : IEventDispatcher
{
    private readonly ILogger<EventDispatcherService> _logger;
    private readonly IMediator _mediator;

    public EventDispatcherService(ILogger<EventDispatcherService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task DispatchAsync(DomainEvent domainEvent, CancellationToken token = default)
    {
        _logger.LogInformation("Publishing {BoundedContext}'s domain events. Event: {event}", "Airline" , domainEvent.GetType().Name);

        var notification = CreateNotification(domainEvent);

        await _mediator.Publish(notification);
    }

    private INotification CreateNotification(DomainEvent domainEvent)
    {
        return (INotification)Activator.CreateInstance(typeof(Notification<>)
            .MakeGenericType(domainEvent.GetType()), domainEvent);
    }
}