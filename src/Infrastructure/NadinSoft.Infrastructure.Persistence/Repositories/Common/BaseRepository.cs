using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NadinSoft.Domain.Common;

namespace NadinSoft.Infrastructure.Persistence.Repositories.Common;

internal abstract class BaseRepository<TEntity>(NadinSoftDbContext db)
    where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _entities = db.Set<TEntity>();
    private protected IQueryable<TEntity> Table => _entities;
    private protected IQueryable<TEntity> TableNoTracking => _entities.AsNoTracking();

    protected virtual async ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _entities.AddAsync(entity, cancellationToken);

    protected virtual async Task<List<TEntity>> ListAllAsync(CancellationToken cancellationToken = default)
        => await _entities.ToListAsync(cancellationToken);

    protected virtual async Task UpdateAsync(Expression<Func<TEntity, bool>> whereClause,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> updateClause,
        CancellationToken cancellationToken = default)
    => await _entities.Where(whereClause).ExecuteUpdateAsync(updateClause, cancellationToken);
    
    protected virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> whereClause,
        CancellationToken cancellationToken = default)
    => await _entities.Where(whereClause).ExecuteDeleteAsync(cancellationToken);
    
}