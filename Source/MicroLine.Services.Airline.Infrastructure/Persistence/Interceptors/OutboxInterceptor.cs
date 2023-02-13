using MapsterMapper;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Infrastructure.Integration;
using MicroLine.Services.Airline.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Interceptors;

internal class OutboxInterceptor : SaveChangesInterceptor
{
    private readonly IMapper _mapper;

    public OutboxInterceptor(IMapper mapper)
    {
        _mapper = mapper;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken token = default)
    {
        AddOutboxMessageEntities(eventData.Context, token);

        return base.SavingChangesAsync(eventData, result, token);
    }

    private void AddOutboxMessageEntities(DbContext dbContext, CancellationToken token)
    {
        dbContext?
            .ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entry => entry.Entity.DomainEvents.Count > 0)
            .ToList()
            .ForEach(entry =>
            {
                var aggregateRoot = entry.Entity;

                foreach (var domainEvent in aggregateRoot.DomainEvents)
                {
                    var integrationEvent = domainEvent.MapToIntegrationEvent(_mapper);

                    if (integrationEvent is null) continue;

                    var outboxMessage = _mapper.Map<OutboxMessage>(integrationEvent);

                    dbContext.Add(outboxMessage);
                }

                aggregateRoot.ClearEvents();
            });
    }

}