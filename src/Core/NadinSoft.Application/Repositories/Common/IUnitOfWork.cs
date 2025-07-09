using NadinSoft.Application.Repositories.ProductRepository;

namespace NadinSoft.Application.Repositories.Common;

public interface IUnitOfWork: IAsyncDisposable, IDisposable
{
    IProductRepository ProductRepository { get; }
    
    Task CommitAsync(CancellationToken cancellationToken = default);
}