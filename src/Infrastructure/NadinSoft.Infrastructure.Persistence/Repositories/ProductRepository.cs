using Microsoft.EntityFrameworkCore;
using NadinSoft.Application.Common;
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

    public async Task<PageResult<ProductEntity>> GetUserProductsAsync(Guid userId, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = TableNoTracking.Where(c => c.UserId.Equals(userId));
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return PageResult<ProductEntity>.Create(items, totalCount, page, pageSize);
    }

    public async Task<ProductEntity?> GetProductDetailByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await TableNoTracking
            .Include(c => c.User)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public async Task<bool> IsProductExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await TableNoTracking.AnyAsync(c => c.Name.Equals(name), cancellationToken);
    }

    public async Task<PageResult<ProductEntity>> GetByNameAsync(string name, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = TableNoTracking.Where(c => c.Name.Contains(name));
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).Include(c => c.User)
            .ToListAsync(cancellationToken);

        return PageResult<ProductEntity>.Create(items, totalCount, page, pageSize);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await base.DeleteAsync(c => c.Id.Equals(id), cancellationToken);
    }

    public async Task<bool> IsUniqueManufactureEmailAndProductDate(string manufactureEmail, DateTime productDate,
        CancellationToken cancellationToken = default)
    {
        return await TableNoTracking.AnyAsync(c =>
            c.ManufactureEmail.Equals(manufactureEmail) && c.ProduceDate.Equals(productDate), cancellationToken);
    }

    public async Task<PageResult<ProductEntity>> GetAllAsync(string? username, int page = 1, int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = TableNoTracking;
        var totalCount = await query.CountAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(username))
            query = query.Where(c => c.User.UserName!.Equals(username));

        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).Include(c => c.User)
            .ToListAsync(cancellationToken);
        return PageResult<ProductEntity>.Create(items, totalCount, page, pageSize);
    }
}