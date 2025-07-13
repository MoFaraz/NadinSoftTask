using Microsoft.EntityFrameworkCore;
using NadinSoft.Domain.Common;
using NadinSoft.Domain.Entities.User;
using NadinSoft.Infrastructure.Persistence.Extensions;

namespace NadinSoft.Infrastructure.Persistence;

public class NadinSoftDbContext(DbContextOptions<NadinSoftDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterEntity<IEntity>(typeof(IEntity).Assembly);
        modelBuilder.ApplyDeleteRestrictBehavior();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NadinSoftDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplyEntityChangeDates();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        ApplyEntityChangeDates();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ApplyEntityChangeDates();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyEntityChangeDates();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    private void ApplyEntityChangeDates()
    {
        var entities = base.ChangeTracker.Entries().Where(c =>
            c is { Entity: IEntity, State: EntityState.Added } or { Entity: IEntity, State: EntityState.Modified });

        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
                ((IEntity)entity.Entity).CreatedDate = DateTime.Now;

            if (entity.State == EntityState.Modified)
                ((IEntity)entity.Entity).ModifiedDate = DateTime.Now;
        }
    }
}