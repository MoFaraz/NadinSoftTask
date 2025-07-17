using Microsoft.EntityFrameworkCore;
using NadinSoft.Domain.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace NadinSoft.Infrastructure.Persistence.Interceptors;

public sealed class AuditInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateShadowDates(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateShadowDates(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }


    private void UpdateShadowDates(DbContext? dbContext)
    {
        if (dbContext is null)
            return;
        var entries = dbContext.ChangeTracker.Entries().Where(c =>
            c is { Entity: IEntity, State: EntityState.Added } or { Entity: IEntity, State: EntityState.Modified });


        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Property("CreatedDate").CurrentValue = DateTime.Now;

            if (entry.State == EntityState.Modified)
                entry.Property("ModifiedDate").CurrentValue = DateTime.Now;
        }
    }
}