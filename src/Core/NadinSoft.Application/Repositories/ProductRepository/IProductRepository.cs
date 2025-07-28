using NadinSoft.Domain.Entities.Product;

namespace NadinSoft.Application.Repositories.ProductRepository;

public interface IProductRepository
{
    Task CreateAsync(ProductEntity product, CancellationToken cancellationToken = default);
    Task<ProductEntity?> GetProductByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ProductEntity>> GetUserProductsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ProductEntity?> GetProductDetailByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsProductExistsAsync(string name, CancellationToken cancellationToken = default);
    Task<List<ProductEntity>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsUniqueManufactureEmailAndProductDate(string manufactureEmail, DateTime productDate, CancellationToken cancellationToken = default);
    Task<PageResult<ProductEntity>> GetAllAsync(string? username, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
}