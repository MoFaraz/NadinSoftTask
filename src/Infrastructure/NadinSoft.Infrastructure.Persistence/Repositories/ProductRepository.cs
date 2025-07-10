using Microsoft.EntityFrameworkCore;
using NadinSoft.Application.Repositories.ProductRepository;
using NadinSoft.Domain.Entities.Product;
using NadinSoft.Infrastructure.Persistence.Repositories.Common;

namespace NadinSoft.Infrastructure.Persistence.Repositories;

internal class ProductRepository(NadinSoftDbContext db) : BaseRepository<ProductEntity>(db), IProductRepository
{
    public async Task CreateAsync(ProductEntity product, CancellationToken cancellationToken = default)
    {
        await base.AddAsync(product, cancellationToken);
    }

    public async Task<ProductEntity?> GetProductByIdForUpdateAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        return await Table.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public async Task<List<ProductEntity>> GetUserProductsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await TableNoTracking.Where(c => c.UserId.Equals(userId)).ToListAsync(cancellationToken);
    }

    public async Task<ProductEntity?> GetProductDetailByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await TableNoTracking
            .Include(c => c.User)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public async Task<bool> IsProductExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await base.TableNoTracking.AnyAsync(c => c.Name.Equals(name), cancellationToken);
    }

    public async Task<List<ProductEntity>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await base.TableNoTracking.Where(c => c.Name.Contains(name))
            .ToListAsync(cancellationToken);
    }
}