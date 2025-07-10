using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Repositories.Common;

namespace NadinSoft.Application.Features.Product.Queries;

public class GetUserProductsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUserProductsQuery, OperationResult<List<GetUserProductsQueryResult>>>
{
    public async ValueTask<OperationResult<List<GetUserProductsQueryResult>>> Handle(GetUserProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await unitOfWork.ProductRepository.GetUserProductsAsync(request.UserId, cancellationToken);

        var result = products.Select(c =>
                new GetUserProductsQueryResult(c.Id, c.Name, c.ManufacturePhone, c.ManufactureEmail, c.ProduceDate))
            .ToList();
        
        return OperationResult<List<GetUserProductsQueryResult>>.SuccessResult(result);
    }
}