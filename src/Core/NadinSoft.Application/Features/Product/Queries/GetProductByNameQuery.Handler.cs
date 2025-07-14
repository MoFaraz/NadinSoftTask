using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Repositories.Common;

namespace NadinSoft.Application.Features.Product.Queries;

public class GetProductByNameQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProductByNameQuery, OperationResult<List<GetProductByNameQueryResult>>>
{
    public async ValueTask<OperationResult<List<GetProductByNameQueryResult>>> Handle(GetProductByNameQuery request,
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.GetByNameAsync(request.SearchTerm, cancellationToken);

        if (product.Count == 0)
            return OperationResult<List<GetProductByNameQueryResult>>.SuccessResult(Enumerable
                .Empty<GetProductByNameQueryResult>().ToList());

        return OperationResult<List<GetProductByNameQueryResult>>.SuccessResult(product.Select(c =>
            new GetProductByNameQueryResult(c.Id, c.ManufacturePhone, c.ManufactureEmail, c.IsAvailable, c.ProduceDate,
                c.UserId)).ToList());
    }
}