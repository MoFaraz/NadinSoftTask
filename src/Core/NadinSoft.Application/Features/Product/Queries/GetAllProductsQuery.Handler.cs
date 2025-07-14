using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Repositories.Common;

namespace NadinSoft.Application.Features.Product.Queries;

public class GetAllProductsQueryHandler(IUnitOfWork unitOfWork): IRequestHandler<GetAllProductsQuery, OperationResult<List<GetAllProductsQueryResult>>>
{
    public async ValueTask<OperationResult<List<GetAllProductsQueryResult>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await unitOfWork.ProductRepository.GetAllAsync(cancellationToken);
        
        var result = products.Select(c => new GetAllProductsQueryResult(c.Name, c.ManufacturePhone, c.ManufactureEmail, c.ProduceDate)).ToList();
        
        return OperationResult<List<GetAllProductsQueryResult>>.SuccessResult(result);
    }
}