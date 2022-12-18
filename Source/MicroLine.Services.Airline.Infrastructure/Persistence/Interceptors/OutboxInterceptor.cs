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

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken token = default)
    {
        await AddOutboxMessageEntitiesAsync(eventData.Context, token);

        return await base.SavingChangesAsync(eventData, result, token);
    }

    private async Task AddOutboxMessageEntitiesAsync(DbContext dbContext, CancellationToken token)
    {
        if (dbContext == null) return;


        var domainEvents = dbContext.ChangeTracker.Entries<AggregateRoot>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            var integrationEvent = domainEvent.MapToIntegrationEvent(_mapper);

            if(integrationEvent is null) continue;

            var outboxMessage = _mapper.Map<OutboxMessage>(integrationEvent);

            await dbContext.AddAsync(outboxMessage, token);
        }
    }

}