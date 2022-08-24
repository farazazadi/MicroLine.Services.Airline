using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Interceptors;

internal class AuditingInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;
    private readonly IDateTime _dateTime;

    public AuditingInterceptor(ICurrentUser currentUser, IDateTime dateTime)
    {
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added) { }
            entry.Entity.SetCreationDetails(_currentUser.UserId, _dateTime.UtcNow);

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || HasChangedOwnedEntities(entry))
                entry.Entity.SetModificationDetails(_currentUser.UserId, _dateTime.UtcNow);
        }
    }

    private bool HasChangedOwnedEntities(EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
    }
}