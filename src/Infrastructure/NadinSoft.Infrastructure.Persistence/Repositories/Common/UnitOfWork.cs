using Mediator;
using NadinSoft.Application.Repositories.Common;
using NadinSoft.Application.Repositories.ProductRepository;
using NadinSoft.Domain.Common;

namespace NadinSoft.Infrastructure.Persistence.Repositories.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly NadinSoftDbContext _db;
    private readonly IPublisher _publisher;

    public UnitOfWork(NadinSoftDbContext db, IPublisher publisher)
    {
        _db = db;
        ProductRepository = new ProductRepository(_db);
        _publisher = publisher;
    }

    public void Dispose()
    {
        _db.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
    }

    public IProductRepository ProductRepository { get; }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);

        await DispatchDomainEventsAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEntities = _db.ChangeTracker.Entries<BaseEntity<Guid>>().Where(c => c.Entity.DomainEvents.Count > 0)
            .ToList();

        var domainEvents = domainEntities.SelectMany(c => c.Entity.DomainEvents).ToList();

        foreach (var entity in domainEntities)
            entity.Entity.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);
    }
}