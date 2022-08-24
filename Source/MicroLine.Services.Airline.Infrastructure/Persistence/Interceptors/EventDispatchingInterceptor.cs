using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Interceptors;

internal class EventDispatchingInterceptor : SaveChangesInterceptor
{
    private readonly IEventDispatcher _eventDispatcher;

    public EventDispatchingInterceptor(IEventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await DispatchEventsAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }


    private async Task DispatchEventsAsync(DbContext dbContext, CancellationToken token = default)
    {

        var domainEvents = dbContext.ChangeTracker.Entries<AggregateRoot>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
            await _eventDispatcher.DispatchAsync(domainEvent, token);
    }
}