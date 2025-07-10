using NadinSoft.Application.Repositories.Common;
using NadinSoft.Application.Repositories.ProductRepository;

namespace NadinSoft.Infrastructure.Persistence.Repositories.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly NadinSoftDbContext _db;

    public UnitOfWork(NadinSoftDbContext db)
    {
        _db = db;
        ProductRepository = new ProductRepository(_db);
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
    }
}